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
        [Route("")]
        public async Task<IActionResult> CreateUsers([FromBody] UserDTO user)
        {
            if (user == null)
                return BadRequest();

            var response = await _userAplication.AddUserAsync(user);
            return Ok(user);
        }


        [HttpPut]
        //[Authorize]
        [Route("")]
        public async Task<IActionResult> UpdateUsers([FromBody] UserDTO user)
        {
            if (user == null)
                return BadRequest();

            var response = await _userAplication.UpdateUserAsync(user);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{username}")]
        //[Authorize]
        public async Task<IActionResult> DeleteUsers([FromRoute] string username)
        {
            var response = await _userAplication.DeleteUserAsync(username);
            return NoContent();
        }

        [HttpGet]
        //[Authorize]
        [Route("{name}")]
        public async Task<IActionResult> FindUsersByName(string name)
        {
            var response = await _userAplication.FindUserByUserName(name);
            return Ok(response);
        }

        /// <summary>
        ///El método GetProfiles,  obtiene los perfiles  de la aplicación. Para poder consumir el método  debe pasar como parámetro el JWT      
        /// </summary>        
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        //[Authorize]
        ////[AllowAnonymous]
        //[HttpGet("GetUsers")]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[SwaggerResponse(200, "OK. Devuelve el objeto solicitado", typeof(Response<IList<ProfileDto>>))]
        [HttpGet("GetUsers")]
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
            var token = jWTAuthenticationManager.Authenticate(userCred.Username, userCred.Password);

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
