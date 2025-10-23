using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.Services.Interfaces;

namespace Practice_Quiz_Generator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionBankController : ControllerBase
    {
        private readonly IQuestionBankService _questionBankService;

        public QuestionBankController(IQuestionBankService questionBankService)
        {
            _questionBankService = questionBankService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportQuestionBank(IFormFile file, string courseTitle)
        {
            await _questionBankService.ImportFromExcelAsync(file, courseTitle);
            return Ok("Questions imported successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _questionBankService.GetAllQuestionsAsync();
            return Ok(result);
        }


        [HttpGet("course/id/{courseId}")]
        public async Task<IActionResult> GetQuestionsByCourseId(string courseId)
        {
            var result = await _questionBankService.GetAllQuestionByCourseIdAsync(courseId);
            return Ok(result);
        }

        [HttpGet("course/title/{courseTitle}")]
        public async Task<IActionResult> GetQuestionsByCourseTitle(string courseTitle)
        {
            var result = await _questionBankService.GetAllQuestionByCourseTitleAsync(courseTitle);
            return Ok(result);
        }

    }
}
