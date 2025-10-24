using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.ServiceConfiguration.MappingExtensions;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Shared.DTOs;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using System.Security.Claims;

namespace Practice_Quiz_Generator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly IFileProcessingService _fileProcessingService;
        private readonly ILogger<QuizController> _logger;

        public QuizController(IQuizService quizService, IFileProcessingService fileProcessingService, ILogger<QuizController> logger)
        {
            _quizService = quizService;
            _fileProcessingService = fileProcessingService;
            _logger = logger;
        }

        //[HttpPost("generatefromfile")]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> GenerateQuizFromFile([FromForm] QuizUploadRequestDto quizUploadRequest)
        //{
        //    if (quizUploadRequest.File == null || quizUploadRequest.File.Length == 0)
        //        return BadRequest("No file uploaded");

        //    var text = await _fileProcessingService.ExtractTextAsync(quizUploadRequest.File);

        //    var request = quizUploadRequest.ToQuizRequestDto();
        //    request.UploadedText = text;
        //    var result = await _quizService.GenerateQuizAsync(request);
        //    return Ok(result);
        //}

        [HttpPost("generate")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateQuizFromFile([FromForm] QuizPersistUploadRequestDto quizUploadRequest)
        {
            //if (quizUploadRequest.File == null || quizUploadRequest.File.Length == 0)
            //    return BadRequest("No file uploaded");
            string text = null;
            if (quizUploadRequest.QuestionSource.Equals("FileUpload", StringComparison.OrdinalIgnoreCase))
            {
                text = await _fileProcessingService.ExtractTextAsync(quizUploadRequest.File);
            }

            var request = quizUploadRequest.ToQuizPersistRequestDto();
            request.UploadedText = text;
            var result = await _quizService.GenerateQuizAsync(request);
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

        [HttpGet("{quizId}/details")]
        public async Task<IActionResult> GetQuizDetails(string quizId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var result = await _quizService.GetQuizDetailsAsync(quizId, userId);

                if (!result.Succeeded)
                {
                    return StatusCode((int)result.StatusCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quiz details for quiz {QuizId}", quizId);
                return StatusCode(500, new { message = "An internal server error occurred" });
            }
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitQuiz([FromBody] SubmitQuizRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var result = await _quizService.SubmitQuizAsync(request, userId);

                if (!result.Succeeded)
                {
                    return StatusCode((int)result.StatusCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting quiz");
                return StatusCode(500, new { message = "An internal server error occurred" });
            }
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetQuizHistory()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var result = await _quizService.GetQuizHistoryAsync(userId);

                if (!result.Succeeded)
                {
                    return StatusCode((int)result.StatusCode, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quiz history");
                return StatusCode(500, new { message = "An internal server error occurred" });
            }
        }

    }
}
