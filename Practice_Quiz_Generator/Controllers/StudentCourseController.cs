using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.Services.Interfaces;

namespace Practice_Quiz_Generator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCourseController : ControllerBase
    {

        private readonly IStudentCourseService _studentCourseService;

        public StudentCourseController(IStudentCourseService studentCourseService)
        {
            _studentCourseService = studentCourseService;
        }

        [HttpGet("{studentid}")]
        public async Task<IActionResult> GetStudentCourses(string studentid)
        {
            var result = await _studentCourseService.GetStudentCoursesAsync(studentid);
            return Ok(result);
        }
    }
}
