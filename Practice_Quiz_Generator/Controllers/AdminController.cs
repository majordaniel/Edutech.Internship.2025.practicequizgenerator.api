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
