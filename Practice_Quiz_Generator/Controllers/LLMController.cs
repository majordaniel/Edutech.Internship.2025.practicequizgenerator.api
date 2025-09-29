using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.Services.Implementations;
using Practice_Quiz_Generator.Application.Services.Interfaces;

namespace Practice_Quiz_Generator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LLMController : ControllerBase
    {
        private readonly IGeminiService _llmService;

        public LLMController(IGeminiService llmService)
        {
            _llmService = llmService;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                return BadRequest("Promt cannot be empty");

            try
            {
                var result = await _llmService.GetLLMResponseAsync(prompt);
                return Ok(new { Response = result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
