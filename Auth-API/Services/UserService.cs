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




        public async Task<RegisterModel> PutUserAsync(string userId, ApplicationUser updatedUser)
        {
            if (userId == null) return new RegisterModel { Message = "Invalid Id" };

            var oldUser = await _userManager.FindByIdAsync(userId);

            if (oldUser is null) return new RegisterModel { Message = "Invalid User" };

            oldUser = _mapper.Map<ApplicationUser>(updatedUser);

            _db.Update(oldUser);
            await _db.SaveChangesAsync();

            RegisterModel user = _mapper.Map<RegisterModel>(oldUser);

            return user;

        }



        public async Task DeleteUserAsync(string userId)
        {
             if (!string.IsNullOrEmpty(userId)) return;

            var user = await _userManager.FindByIdAsync(userId);
            _db.Remove(user);
            await _db.SaveChangesAsync();
        }


    }
}