
using System.Threading.Tasks;
using Auth_API.Models;
using Auth_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth_API.Controllers
{
    // [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();

            if (users == null) return BadRequest(ModelState);

            return Ok(users);
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> GetUser([FromBody] string id)
        {
            // if (ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.GetUserAsync(id);

            if (result is null) return BadRequest(result.Message);

            return Ok(result);
        }



        [HttpPost("Add")]
        public async Task<IActionResult> AddUser([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //Register User and Get the Result
            var result = await _authService.RegisterAsync(model);

            //In case Some error => BadRequest
            if (!result.IsAuthenticated) return BadRequest(result.Message);

            //In case success return Ok with the result OR Object with some info (if Needed!)
            //return Ok(new {Token = result.Token , ExpiresOn= result.ExpiresOn});
            return Ok(result);
        }


        [HttpPut("Edit")]
        public async Task<IActionResult> EditUser(string userId, EditUserModel user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.PutUserAsync(userId, user);

            if (result is null) return BadRequest(result.Message);
            return Ok(result);
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUser(string userId)
        {


            if (!await _userService.DeleteUserAsync(userId)) return BadRequest(new { Message = "Couldn't Delete User !" });

            return Ok(new { Message = "User deleted successfully."});

        }






        #region Get UserList and RoleList


        [HttpGet("Roles")]
        public async Task<IActionResult> UsersAndRolesAsync()
        {
            var roleList = await _userService.GetRolesList();
            var usersList = await _userService.GetUsersList();

            if (roleList.Count < 0 || usersList.Count < 0) return BadRequest();

            return Ok(new { Roles = roleList, Users = usersList });

        }

        #endregion





        #region Add User To Role Method


        [HttpPost("ManageRole")]
        public async Task<IActionResult> AddToRoleAsync([FromBody] AddRoleModel model)
        {
            //Check the Model State(Annotaions)
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //AddRole and Get the Result
            var result = await _userService.AddRoleAsync(model);

            //In case Some error => BadRequest
            if (string.IsNullOrEmpty(result)) return BadRequest(result);

            //In case success return Ok with the result OR Object with some info (Needed!)
            //return Ok(new {Token = result.Token , ExpiresOn= result.ExpiresOn});
            return Ok(result);
        }


        #endregion

    }
}