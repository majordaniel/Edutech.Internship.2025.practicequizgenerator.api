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
            return Ok(new { message = "You are authorized!" });
        }

        //role based test

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok("You are an Admin!");
        }

        //forgot password endpoint

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] string email)
        {
            var token = _authService.ForgotPassword(email);

            if (token == null)
            {
                return NotFound(new { message = "Email not found" });
            }

         
            return Ok(new { message = "Password reset token generated", token = token });
        }


        public class ResetPasswordRequest
        {
            public string Email { get; set; }
            public string Token { get; set; }
            public string NewPassword { get; set; }
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var success = _authService.ResetPassword(request.Email, request.Token, request.NewPassword);

            if (!success)
            {
                return BadRequest(new { message = "Invalid or expired token" });
            }

            return Ok(new { message = "Password has been reset successfully" });
        }
    }
}
