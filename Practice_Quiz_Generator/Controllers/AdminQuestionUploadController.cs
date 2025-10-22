using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Shared.DTOs.Request;


namespace Practice_Quiz_Generator.Controllers
{


    [ApiController]
    [Route("api/admin/questions")]
    [Authorize(Roles = "Admin")]
    public class AdminQuestionUploadController : ControllerBase
    {
        private readonly IQuestionImportService _importService;

        public AdminQuestionUploadController(IQuestionImportService importService)
        {
            _importService = importService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] BulkUploadRequestDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("File is required.");

            var adminId = User?.Identity?.Name ?? "UnknownAdmin";
            var jobId = await _importService.StartImportJobAsync(dto.File, dto.CourseId, adminId);

            return Accepted(new { jobId });
        }

        [HttpGet("upload/status/{jobId}")]
        public async Task<IActionResult> GetStatus(Guid jobId)
        {
            var result = await _importService.GetJobStatusAsync(jobId);
            if (result == null)
                return NotFound("Job not found.");

            return Ok(result);
        }
    }

}
