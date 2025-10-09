using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.ServiceConfiguration.MappingExtensions;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Shared.DTOs;
using Practice_Quiz_Generator.Shared.DTOs.Request;

namespace Practice_Quiz_Generator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly IFileProcessingService _fileProcessingService;

        public QuizController(IQuizService quizService, IFileProcessingService fileProcessingService)
        {
            _quizService = quizService;
            _fileProcessingService = fileProcessingService;

        }

        [HttpPost("generatefromfile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> GenerateQuizFromFile([FromForm] QuizUploadRequestDto quizUploadRequest)
        {
            if (quizUploadRequest.File == null || quizUploadRequest.File.Length == 0)
                return BadRequest("No file uploaded");

            var text = await _fileProcessingService.ExtractTextAsync(quizUploadRequest.File);

            var request = quizUploadRequest.ToQuizRequestDto();
            request.UploadedText = text;
            var result = await _quizService.GenerateQuizAsync(request);
            return Ok(result);
        }

        [HttpPost("createfromfile/persist")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateQuizFromFile([FromForm] QuizPersistUploadRequestDto quizUploadRequest)
        {
            if (quizUploadRequest.File == null || quizUploadRequest.File.Length == 0)
                return BadRequest("No file uploaded");

            var text = await _fileProcessingService.ExtractTextAsync(quizUploadRequest.File);

            var request = quizUploadRequest.ToQuizPersistRequestDto();
            request.UploadedText = text;
            var result = await _quizService.CreateQuizAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuizById(string id)
        {
            var result = await _quizService.GetQuizByIdAsync(id);
            //return StatusCode(result.StatusCode, result);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllUserQuizzes(string userId)
        {
            var result = await _quizService.GetAllUserQuizzesAsync(userId);
            return Ok( result);
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitQuiz([FromBody] QuizSubmissionRequestDto request)
        {
            var result = await _quizService.SubmitQuizAsync(request);
            return Ok(result);
        }

        [HttpGet("results/{quizId}")]
        public async Task<IActionResult> GetQuizResults(string quizId, [FromQuery] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("UserId is required");

            var result = await _quizService.GetQuizResultsAsync(quizId, userId);
            return Ok(result);
        }
    }
}
