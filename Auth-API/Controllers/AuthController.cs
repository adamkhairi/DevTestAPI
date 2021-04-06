using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth_API.Models;
using Auth_API.Services;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace Auth_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {


        private readonly IAuthService _authService;


        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }



        #region Registration Method (Create Token and register user)


        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            //Check the Model State(Annotaions)
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //Register User and Get the Result
            var result = await _authService.RegisterAsync(model);

            //In case Some error => BadRequest
            if (!result.IsAuthenticated) return BadRequest(result.Message);

            //In case success return Ok with the result OR Object with some info (Needed!)
            //return Ok(new {Token = result.Token , ExpiresOn= result.ExpiresOn});
            return Ok(result);

        }


        #endregion





        #region Login Method (Get Token)


        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] TokenRequestModel model)
        {
            //Check the Model State(Annotaions)
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //Login User and Get the Result
            var result = await _authService.GetTokenAsync(model);

            //In case Some error => BadRequest
            if (!result.IsAuthenticated) return BadRequest(result.Message);

            //In case success return Ok with the result OR Object with some info (Needed!)
            //return Ok(new {Token = result.Token , ExpiresOn= result.ExpiresOn});
            return Ok(result);
        }


        #endregion



        #region Get UserList and RoleList


        [Authorize(Roles = "Admin")]
        [HttpGet("Roles")]
        public async Task<IActionResult> AddToRoleAsync()
        {
            var roleList = await _authService.GetRolesList();
            var usersList = await _authService.GetUsersList();

            if (roleList.Count < 0 || usersList.Count < 0) return BadRequest();

            return Ok(new { Roles = roleList, Users = usersList });

        }

        #endregion





        #region Add User To Role Method


        [Authorize(Roles = "Admin")]
        [HttpPost("Roles")]
        public async Task<IActionResult> AddToRoleAsync([FromBody] AddRoleModel model)
        {
            //Check the Model State(Annotaions)
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //AddRole and Get the Result
            var result = await _authService.AddRoleAsync(model);

            //In case Some error => BadRequest
            if (string.IsNullOrEmpty(result)) return BadRequest(result);

            //In case success return Ok with the result OR Object with some info (Needed!)
            //return Ok(new {Token = result.Token , ExpiresOn= result.ExpiresOn});
            return Ok(result);
        }


        #endregion
    }
}
