using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Practice_Quiz_Generator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet("login")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult TestAdmin()
        {
            return Ok(" You are authorized as Admin or SuperAdmin");
        }
    }
}

//[ApiController]
//[Route("api/[controller]")]
//public class AdminController : ControllerBase
//{
//    // Only Admins can access
//    [HttpGet("users")]
//    [Authorize(Roles = "Admin")]
//    public IActionResult GetAllUsers()
//    {
//        // Replace with actual DB call
//        var users = new[]
//        {
//            new { Id = 1, Name = "Admin User", Role = "Admin" },
//            new { Id = 2, Name = "Student User", Role = "Student" }
//        };
//        return Ok(users);
//    }
//}