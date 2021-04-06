using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth_API.Helpers;
using Auth_API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IOptions<Jwt> _jwt;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IOptions<Jwt> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwt = jwt;
        }




        #region  Registration

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            //Check For Duplicate Email and Username
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel() { Message = "Email is already Registered" };

            if (await _userManager.FindByNameAsync(model.UserName) is not null)
                return new AuthModel() { Message = "Username is already Registered" };

            //Create new User

            //var user = new ApplicationUser()
            //{
            //    UserName = model.UserName,
            //    Email = model.Email,
            //    FirstName = model.FirstName,
            //    LastName = model.LastName
            //};

            var user = _mapper.Map<ApplicationUser>(model);

            // !!/TODO ==> Fix Mapper !!
            //_mapper.Map<ApplicationUser>(model);

            //Create User and Hash Password While Creating
            var result = await _userManager.CreateAsync(user, model.Password);


            //If Not succeed
            if (!result.Succeeded)
            {
                var errors = string.Empty;

                //Get Errors Description
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}, ";
                }

                //Return Errors
                return new AuthModel() { Message = errors };

            }

            //Add new Users To [User] Role
            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                Email = user.Email,
                UserName = user.UserName,
                //TODO To Change ==>
                Roles = new List<string> { "User" },
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };
        }

        #endregion




        #region  Create a Token JWT

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("uid", user.Id)
                }
                .Union(userClaims)
                .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Value.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken
            (
                issuer: _jwt.Value.Issuer,
                audience: _jwt.Value.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.Value.DurationInDays
                ),

                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }


        #endregion




        #region Get Token From Login

        public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email/Password is Incorrect !";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);


            //TODO ==> Mapper
            authModel = _mapper.Map<AuthModel>(user);

            //authModel.Email = user.Email;
            //authModel.UserName = user.UserName;
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            return authModel;
        }


        #endregion




        #region Add User to Role

        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            //Get User
            var user = await _userManager.FindByIdAsync(model.UserId);

            //Check User and Role if Exist
            if (user is null || !await _roleManager.RoleExistsAsync(model.Role)) return "Invalid user ID Or Role";

            //Check if User have Role
            if (await _userManager.IsInRoleAsync(user, model.Role)) return "User Already Have this Role !";

            //Assign Role To User
            var result = await _userManager.AddToRoleAsync(user, model.Role);

            //Check Result if succeeses
            return result.Succeeded ? string.Empty : "Something Wrong !!";
        }


        #endregion




        #region GetRoles

        public async Task<List<object>> GetRolesList()
        {
            var rolesList = new List<object>();

            var roles = await _roleManager.Roles.ToListAsync();

            roles.ForEach(r =>
            {
                var role = new
                {
                    roleId = r.Id,
                    name = r.Name
                };
                rolesList.Add(role);
            });
            return rolesList;
        }

        #endregion




        #region GetUsers

        public async Task<List<object>> GetUsersList()
        {
            var usersList = new List<Object>();

            var users = await _userManager.Users.ToListAsync();
            users.ForEach(u =>
           {
               var user = new
               {
                   userId = u.Id,
                   Username = u.UserName,
               };
               usersList.Add(user);
           });
            return usersList;
        }

        #endregion
    }
}
