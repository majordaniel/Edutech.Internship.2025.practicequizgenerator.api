using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Shared.DTOs.Request;

namespace Practice_Quiz_Generator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TheoryGradingController : Controller
    {
        private readonly ITheoryGradingService _gradingService;
        private readonly ILogger<TheoryGradingController> _logger;

        public TheoryGradingController(
            ITheoryGradingService gradingService, ILogger<TheoryGradingController> logger)
        {
            _gradingService = gradingService;
            _logger = logger;
        }

        [HttpPost("grade")]
        public async Task<IActionResult> Grade([FromBody] TheoryGradingRequestDto theoryGradingRequest)
        {
            try
            {
                if (theoryGradingRequest == null)
                    return BadRequest("Invalid request");

                var result = await _gradingService.GradeAsync(theoryGradingRequest);
                return Ok(result);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TheoryGrading endpoint");
                return StatusCode(500, new { message = "An internal server error occured" });
            }
        }
    }
}
