using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth_API.Data;
using Auth_API.Helpers;
using Auth_API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Auth_API.Services
{

    public class UserService : IUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AutoMapper.IMapper _mapper;
        private readonly IOptions<Jwt> _jwt;
        // private readonly AuthService _authService;


        private readonly AppDbContext _db;

        public UserService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IOptions<Jwt> jwt)
        {
            _db = db;
            // _authService = authService;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwt = jwt;
        }



        public async Task<RegisterModel> GetUserAsync(string userId)
        {
            if (userId is null)
                return new RegisterModel { Message = "Invalid User Id" };


            var user = await _userManager.FindByIdAsync(userId);


            if (await _userManager.FindByIdAsync(userId) is null)
                return new RegisterModel { Message = $"Couldn't find user with id  {userId}" };

            // var model = new RegisterModel();

            //user.Email = model.Email;  ...
            var model = _mapper.Map<RegisterModel>(user);
            return model;

        }



        public async Task<List<RegisterModel>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            if (users.Count <= 0) return new List<RegisterModel>();

            // var usersModel = _mapper.Map<List<RegisterModel>>(users);
            List<RegisterModel> list = new List<RegisterModel>();
            users.ForEach(u =>
           {
               var userModell = _mapper.Map<RegisterModel>(u);
               list.Add(userModell);
           });

            return list;

        }




        public async Task<RegisterModel> PutUserAsync(string userId, EditUserModel updatedUser)
        {
            var registerModel = new RegisterModel();
            registerModel = _mapper.Map<RegisterModel>(updatedUser);

            if (userId == null)
            {
                registerModel.Message = "Invalid Id";
                return registerModel;
            }

            var oldUser = await _userManager.FindByIdAsync(userId);

            if (oldUser is null)
            {
                registerModel.Message = "Invalid User";
                return registerModel;
            }

            if (string.IsNullOrEmpty(updatedUser.Password))
            {
                registerModel.Message = "Please enter your password";
                return registerModel;
            }
            else
            {
                if (!string.IsNullOrEmpty(updatedUser.NewPassword))
                {
                    if (updatedUser.NewPassword != updatedUser.ConfirmNewPassword)
                    {
                        registerModel.Message = "New Passwod and Confirm New Password are not same";
                        return registerModel;
                    }
                    var res = await _userManager.ChangePasswordAsync(oldUser, updatedUser.Password, updatedUser.NewPassword);
                    if (!res.Succeeded)
                    {
                        registerModel.Password = "";
                        registerModel.Message = "Couldn't change your password. Please try again !";
                        return registerModel;
                    }
                }
                registerModel.Password = "";


                // oldUser = _mapper.Map<ApplicationUser>(updatedUser);

                oldUser.Email = string.IsNullOrEmpty(updatedUser.Email) ? oldUser.Email : updatedUser.Email;
                oldUser.FirstName = string.IsNullOrEmpty(updatedUser.FirstName) ? oldUser.FirstName : updatedUser.FirstName;
                oldUser.LastName = string.IsNullOrEmpty(updatedUser.LastName) ? oldUser.LastName : updatedUser.LastName;
                oldUser.UserName = string.IsNullOrEmpty(updatedUser.UserName) ? oldUser.UserName : updatedUser.UserName;
                oldUser.PhoneNumber = string.IsNullOrEmpty(updatedUser.PhoneNumber) ? oldUser.PhoneNumber : updatedUser.PhoneNumber;

                //oldUser.Email = string.IsNullOrEmpty(updatedUser.Email) ? oldUser.Email : updatedUser.Email;
                // var oldUser1 = _mapper.Map<ApplicationUser>(updatedUser);
                // var uu = _mapper.Map<ApplicationUser>(oldUser1);




                var result = await _userManager.UpdateAsync(oldUser);

                if (!result.Succeeded)
                {
                    registerModel.Message = result.Errors.AsEnumerable().ToString() + ", Somthing whent wrong !";
                    return registerModel;
                }

                //_db.Update(oldUser);
                await _db.SaveChangesAsync();

                RegisterModel user = _mapper.Map<RegisterModel>(oldUser);

                return user;
            };
        }



        public async Task<bool> DeleteUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return false;

            var user = await _userManager.FindByIdAsync(userId);

           var result = await _userManager.DeleteAsync(user);
            // _db.Remove(user);
            if(!result.Succeeded) return false;
            return true;
        }







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