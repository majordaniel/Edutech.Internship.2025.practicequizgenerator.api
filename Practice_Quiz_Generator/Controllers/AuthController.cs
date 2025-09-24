using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Shared.DTOs.Request;

namespace Practice_Quiz_Generator.Controllers
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

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserRequestDto userRequest)
        {
            var result = await _authService.RegisterAsync(userRequest);
            return Ok(result);
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] CreateUserRequestDto dto)
        //{
        //    var response = await _authService.RegisterAsync(dto);
        //    return StatusCode(response.StatusCode, response);
        //}

        //[HttpPost("forgotpassword")]
        //public async Task<IActionResult> ForgotPassword([FromBody] string email)
        //{
        //    var response = await _authService.ForgotPasswordAsync(email);
        //    return StatusCode(response.StatusCode, response);
        //}

        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var response = await _authService.ConfirmEmailAsync(email, token);
            return StatusCode(response.StatusCode, response);
        }

    }
}
