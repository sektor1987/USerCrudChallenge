using Auth.Demo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UserCrudApiChallenge.Application.DTO;
using UserCrudApiChallenge.Application.Interface;
using UserCrudApiChallenge.Domain.Entity;

namespace UserCrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserAplication _userAplication;
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;
        private readonly ITokenRefresher tokenRefresher;
        public UserController(IUserAplication userAplication, IJWTAuthenticationManager jWTAuthenticationManager, ITokenRefresher tokenRefresher)
        {
            _userAplication = userAplication;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
            this.tokenRefresher = tokenRefresher;
        }

        [HttpPost]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> CreateUsers([FromBody] UserDTO user)
        {
            if (user == null)
                return BadRequest();

            var response = await _userAplication.AddUserAsync(user);
            return Ok(user);
        }


        [HttpPut]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> UpdateUsers([FromBody] UserDTO user)
        {
            if (user == null)
                return BadRequest();

            var response = await _userAplication.UpdateUserAsync(user);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUsers([FromRoute] string id)
        {
            var response = await _userAplication.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> FindUserById(string id)
        {
            var response = await _userAplication.FindUserById(id);
            return Ok(response);
        }

        [HttpGet("GetUsers")]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            List<UserDTO> _response = new();
            _response = await _userAplication.GetUsers();
            return Ok(_response);
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserCred userCred)
        {
            var token = jWTAuthenticationManager.Authenticate(userCred.Email, userCred.Password);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshCred refreshCred)
        {
            var token = tokenRefresher.Refresh(refreshCred);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }
    }
}
