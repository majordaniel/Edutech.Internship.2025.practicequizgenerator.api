using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Service;

namespace Practice_Quiz_Generator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _authService.Authenticate(request.Email, request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = _authService.GenerateJwt(user);

            var response = new LoginResponse
            {
                Token = token,
                ExpiresIn = 3600,
                UserId = user.UserId.ToString(), 
                Name = user.FullName,
                Role = user.Role
            };

            return Ok(response);
        }

        //securing a test endpoint.
        [Authorize]
        [HttpGet("secure-test")]
        public IActionResult SecureTest()
        {
            return Ok(new { message = "✅ You are authorized!" });
        }

        //role based test

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok("You are an Admin!");
        }

    }
}
