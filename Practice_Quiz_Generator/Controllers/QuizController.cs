using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GenerateQuizFromFile([FromForm] QuizUploadRequestDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("No file uploaded");

            var text = await _fileProcessingService.ExtractTextAsync(dto.File);

            var request = new QuizRequestDto
            {
                NumberOfQuestions = dto.NumberOfQuestions,
                UploadedText = text,
                QuestionType = dto.QuestionType ?? "MCQ"
            };

            var quiz = await _quizService.GenerateQuizAsync(request);
            return Ok(quiz);
        }

    }
}
