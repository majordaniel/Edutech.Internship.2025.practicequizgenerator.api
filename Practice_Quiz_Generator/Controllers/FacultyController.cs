using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.Services.Interfaces;

namespace Practice_Quiz_Generator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        private readonly IFacultyService _facultyService;

        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFaculty()
        {
            var result = await _facultyService.GetAllFacultiesAsync();
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllFaculty()
        {
            var result = await _facultyService.GetAllFacultiesAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFaculty()
        {
            var result = await _facultyService.GetAllFacultiesAsync();
            return Ok(result);
        }



    }
}
