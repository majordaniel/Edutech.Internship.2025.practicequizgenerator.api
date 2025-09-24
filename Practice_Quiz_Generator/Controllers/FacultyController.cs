using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Shared.DTOs.Request;

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


        [HttpGet("facultyname")]
        public async Task<IActionResult> GetFacultyByName(string name)
        {
            var result = await _facultyService.GetFacultyByNameAsync(name);
            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetFacultyById(string id)
        {
            var result = await _facultyService.GetFacultyByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("createfaculty")]
        public async Task<IActionResult> CreateFaculty(FacultyRequestDto facultyRequest)
        {
            var result = await _facultyService.CreateFacultyAsync(facultyRequest);
            return Ok(result);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateFaculty(string id, FacultyRequestDto facultyRequest)
        {
            var result = await _facultyService.UpdateFacultyAsync(id, facultyRequest);
            return Ok(result);
        }

        // Reminder --> Add soft delete

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteFaculty(string id)
        {
            var result = await _facultyService.DeleteFacultyAsync(id);
            return Ok(result);
        }




    }
}
