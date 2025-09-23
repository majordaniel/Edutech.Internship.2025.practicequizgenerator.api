using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuizController : ControllerBase
    {
        private readonly IQuizValidationService _quizValidationService;
        private readonly IQuizGenerationService _quizGenerationService;
        private readonly IUserService _userService;
        private readonly IStudentCourseService _studentCourseService;

        public QuizController(IQuizValidationService quizService, IQuizGenerationService quizGenerationService, IUserService userService, IStudentCourseService studentCourseService)
        {
            _quizValidationService = quizService;
            _quizGenerationService = quizGenerationService;
            _userService = userService;
            _studentCourseService = studentCourseService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateQuiz([FromBody] QuizGenerationRequestDto request)
        {
            var studentId = _userService.GetCurrentUserId();

            if (!await _quizValidationService.ValidateRequest(request, studentId))
            {
                return BadRequest("Invalid quiz generation request.");
            }

            await _quizGenerationService.GenerateQuizAsync(request);

            return Accepted(new { quizRequestId = Guid.NewGuid().ToString() });
        }

        // This is the new endpoint for the quiz setup form.
        [HttpGet("setup")]
        public async Task<IActionResult> GetQuizSetupData()
        {
            var studentId = _userService.GetCurrentUserId();
            
            // Get the list of courses from the database.
            var studentCourses = await _studentCourseService.GetStudentCourseIdsAsync(studentId);

            // Hardcoded static data for the form.
            var availableQuestionTypes = new List<string> { "MultipleChoice", "Theory" };
            var availableSources = new List<string> { "PastExams", "UploadedMaterials" };
            const int minQuestions = 5;
            const int maxQuestions = 50;

            // Return a single object with all the data.
            return Ok(new 
            {
                Courses = studentCourses,
                QuestionTypes = availableQuestionTypes,
                Sources = availableSources,
                MinQuestions = minQuestions,
                MaxQuestions = maxQuestions
            });
        }
    }
}