using GoldinAccountManager.JwtAuthentification.Interface;
using GoldinAccountManager.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoldinAccountManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {

        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticateController( IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                var login = await _authenticationRepository.LoginAsync(model);
                if (login.Valid)
                {
                    return Ok(new
                    {
                        token = login.Token,
                        expiration = login.Expiration
                    });
                }
                else
                    return Unauthorized();

            }
            else
                return Unauthorized();

        }

        [HttpPost]
        [Route("RegisterUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _authenticationRepository.GetIdentityUserByUsernameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            var registerUser = await _authenticationRepository.RegisterUserAsync(model);

            if (registerUser.Valid)
                return Ok(new Response { Status = "Success", Message = registerUser.Message });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = registerUser.Message });

        }

        [HttpPost]
        [Route("RegisterAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _authenticationRepository.GetIdentityUserByUsernameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            
            var registerUser = await _authenticationRepository.RegisterAdminAsync(model);

            if (registerUser.Valid)
                return Ok(new Response { Status = "Success", Message = registerUser.Message });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = registerUser.Message });
           
        }

    }
}
