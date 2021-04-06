
using System.Threading.Tasks;
using Auth_API.Models;
using Auth_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth_API.Controllers
{
    [Authorize(Roles = "Admin")]
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

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();

            if (users == null) return BadRequest(ModelState);

            return Ok(users);
        }


        [HttpGet("{id}")]
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

            //In case success return Ok with the result OR Object with some info (Needed!)
            //return Ok(new {Token = result.Token , ExpiresOn= result.ExpiresOn});
            return Ok(result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string userId, ApplicationUser user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.PutUserAsync(userId, user);

            if (result is null) return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userId)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _userService.DeleteUserAsync(userId);

            return Ok();

        }
    }
}