using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.Services;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly QuizService _quizService;

        public QuizController(QuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet("form-config")]
        public async Task<ActionResult<QuizFormConfigurationDto>> GetQuizFormConfiguration()
        {
            var config = await _quizService.GetQuizFormConfigurationAsync(User);
            return Ok(config);
        }
    }
}