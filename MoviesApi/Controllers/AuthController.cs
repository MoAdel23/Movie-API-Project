using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Helpers;
using MoviesApi.Interfaces;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterModel model)
        {

           var result  = await _authServices.RegisterAsync(model);

            if (!result.IsAuhtenticated)
                return BadRequest(result.Message);

            return Ok(result);
            
        }


        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {

            var result = await _authServices.GetTokenAsync(model);

            if (!result.IsAuhtenticated)
                return BadRequest(result.Message);

            return Ok(new
            {
                Token = result.Token,
                ExpiresOn = result.ExpiresOn
            });
        }

        [HttpPost("AddRole")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {

            var result = await _authServices.AddroleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);  
        }
    }
}
