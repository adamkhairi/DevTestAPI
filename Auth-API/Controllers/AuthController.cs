using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth_API.Models;
using Auth_API.Services;
using Auth_API.Services.Auth;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace Auth_API.Controllers
{
    /// <inheritdoc />
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
        /// <summary>
        /// Add New User and Return Token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(AuthModel) ,StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthModel) ,StatusCodes.Status400BadRequest)]
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
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
        /// <summary>
        /// Return Login Token
        /// </summary>
        /// <param name="model"></param>
        /// <returns type="AuthModel"></returns>
        [ProducesResponseType(typeof(AuthModel) ,StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthModel) ,StatusCodes.Status400BadRequest)]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] TokenRequestModel model)
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
    }
}