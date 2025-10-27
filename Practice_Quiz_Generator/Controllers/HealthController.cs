using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Practice_Quiz_Generator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("/health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "Healthy", service = "Practice Quiz Generator", timeStamp = DateTime.UtcNow });
        }
    }
}
