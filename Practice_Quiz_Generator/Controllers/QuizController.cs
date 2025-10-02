using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class QuizController : ControllerBase
    {
        private readonly IQuizValidationService _quizValidationService;
        private readonly IQuizGenerationService _quizGenerationService;
        private readonly IUserService _userService;
        private readonly IStudentCourseService _studentCourseService;

        private readonly IQuizSetupService _quizSetupService;

        private readonly IQuizSubmissionService _quizSubmissionService;

        public QuizController(IQuizValidationService quizService, IQuizGenerationService quizGenerationService, IUserService userService, IStudentCourseService studentCourseService, IQuizSetupService quizSetupService, IQuizSubmissionService quizSubmissionService)
        {
            _quizValidationService = quizService;
            _quizGenerationService = quizGenerationService;
            _userService = userService;
            _studentCourseService = studentCourseService;
            _quizSubmissionService = quizSubmissionService;
            _quizSetupService = quizSetupService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateQuiz([FromBody] QuizGenerationRequestDto request)
        {
            var studentId = _userService.GetCurrentUserId();

            if (!await _quizValidationService.ValidateRequest(request, studentId))
            {
                return BadRequest("Invalid quiz generation request.");
            }

            var quizRequestId = await _quizGenerationService.GenerateQuizAsync(request);
            return Accepted(new { quizRequestId });
        }

        // This is the new endpoint for the quiz setup form.
        [HttpGet("setup")]
        public async Task<IActionResult> GetQuizSetupData()
        {
            var studentId = _userService.GetCurrentUserId();
            var setupData = await _quizSetupService.GetQuizSetupDataAsync(studentId);

            return Ok(setupData);
        }

        [HttpPost("submit")]
        [ProducesResponseType(typeof(QuizResultResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SubmitQuiz([FromBody] QuizSubmissionDto submissionDto)
        {
            if (submissionDto == null || submissionDto.Answers.Count() == 0)
            {
                return BadRequest("Submission must contain answers.");
            }

            var result = await _quizSubmissionService.SubmitAndGradeAsync(submissionDto);
            
            return Ok(result);
        }
    }
}
