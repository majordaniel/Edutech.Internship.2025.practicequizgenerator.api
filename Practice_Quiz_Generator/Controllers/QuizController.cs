using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.Services;
using Practice_Quiz_Generator.Domain.Models;
using System.Security.Claims;

namespace Practice_Quiz_Generator.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly QuizService _quizService;
        private readonly QuizGenerationService _quizGenerationService;

        public QuizController(QuizService quizService, QuizGenerationService quizGenerationService)
        {
            _quizService = quizService;
            _quizGenerationService = quizGenerationService;
        }

        [HttpGet("form-config")]
        public async Task<ActionResult<QuizFormConfigurationDto>> GetQuizFormConfiguration()
        {
            var config = await _quizService.GetQuizFormConfigurationAsync(User);
            return Ok(config);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateQuiz([FromBody] QuizGenerationRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userIdClaim?.Value ?? "0");
            var (isValid, message) = await _quizGenerationService.ValidateRequestAsync(request, userId);

            if (!isValid)
            {
                return BadRequest(message);
            }

            var quizRequestId = _quizGenerationService.EnqueueQuizGenerationRequest(request, userId);

            return Accepted(new { quizRequestId = quizRequestId, message = "Quiz generation request has been accepted and is being processed." });
        }
    }
}