using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        // Any logged-in user (student) can access
        [HttpGet("profile")]
        [Authorize]
        public IActionResult GetStudentProfile()
        {
            // Example: get logged-in user's ID from token
            var userId = User.FindFirst("sub")?.Value;
            return Ok(new { UserId = userId, Name = "Student User", Role = "Student" });
        }
    }

}
