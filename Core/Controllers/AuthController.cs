using Core.Services.Contracts;
using Data.DataTransfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
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

        [HttpPost("/Register")]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequest request)
        {
            var result = await _authService.RegisterUserAsync(request);

            if (result.IsSuccess)
            {   
                //Change result to different 
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateUserAsync(AuthUserRequest request)
        {
            var result = await _authService.AuthenticateAsync(request);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [Authorize]
        [HttpGet]
        public IActionResult VerifyAuthentication() 
        {
            return Ok("Authentication processed.");
        }
    }
}
