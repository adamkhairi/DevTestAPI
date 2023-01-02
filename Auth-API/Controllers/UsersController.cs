
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth_API.Models;
using Auth_API.Services;
using Auth_API.Services.Auth;
using Auth_API.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth_API.Controllers
{
    // [Authorize(Roles = "Admin")]
    /// <inheritdoc />
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UsersController(IUserService userService, IAuthService authService)
        {
            _authService = authService;
            _userService = userService;
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<UserModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        // [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAll();

            if (users == null) return BadRequest(ModelState);

            return Ok(users);
        }
        

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUser(string id)
        {
            // if (ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.Get(id);

            if (string.IsNullOrWhiteSpace(result.Id)) return BadRequest(result.Message);

            return Ok(result);
        }

        /// <summary>
        /// Add New User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(AuthModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddUser([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // var usermodel =
            //Register User and Get the Result
            var result = await _authService.RegisterAsync(model);

            //In case Some error => BadRequest
            if (!result.IsAuthenticated) return BadRequest(result.Message);

            //In case success return Ok with the result OR Object with some info (if Needed!)
            //return Ok(new {Token = result.Token , ExpiresOn= result.ExpiresOn});
            return Ok(result);
        }

        /// <summary>
        /// Edit User 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [Authorize]
        [HttpPut("[action]")]
        public async Task<IActionResult> EditUser(string id, EditUserModel user)
        {
            if (!ModelState.IsValid || id != user.Id) return BadRequest(ModelState);


            var result = await _userService.Put(user);

            if (!string.IsNullOrEmpty(result.Message)) return BadRequest(result.Message);
            return Ok(result);
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [Authorize] 
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!await _userService.Delete(id)) return BadRequest(new {Message = "Couldn't Delete User !"});

            return Ok(new {Message = "User deleted successfully."});
        }


        #region Get UserList and RoleList

       /// <summary>
       /// Return List of Users and Roles
       /// </summary>
       /// <returns></returns>
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> UsersAndRoles()
        {
            var roleList = await _userService.GetRolesList();
            var usersList = await _userService.GetUsersList();

            if (roleList.Count < 0 || usersList.Count < 0) return BadRequest();

            return Ok(new {Roles = roleList, Users = usersList});
        }

        #endregion


        #region Add User To Role Method

        /// <summary>
        /// Add User To Role (User Id, Role Name)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [Authorize]  
        [HttpPost("[action]")]
        public async Task<IActionResult> AddToRole([FromBody] AddRoleModel model)
        {
            //Check the Model State(Annotaions)
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //AddRole and Get the Result
            var result = await _userService.AddRoleAsync(model);

            //In case Some error => BadRequest
            if (result?.Length < 0) return BadRequest(result.Length);

            //In case success return Ok with the result OR Object with some info (Needed!)
            //return Ok(new {Token = result.Token , ExpiresOn= result.ExpiresOn});
            return Ok(result);
        }

        #endregion

    }
}