using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Shared.DTOs.Request;

namespace Practice_Quiz_Generator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartment()
        {
            var result = await _departmentService.GetAllDepartmentAsync();
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActiveDepartmentAsync()
        {
            var result = await _departmentService.GetAllActiveDepartmentAsync();
            return Ok(result);
        }



        //Reminder --> GetActiveDpt.
        [HttpGet("departmentname")]
        public async Task<IActionResult> GetDepartmentByName(string name)
        {
            var result = await _departmentService.GetDepartmentByNameAsync(name);
            return Ok(result);
        }


        [HttpGet("id")]
        public async Task<IActionResult> GetDepartmentById(string id)
        {
            var result = await _departmentService.GetDepartmentByIdAsync(id);
            return Ok(result);
        }


        [HttpPut("id")]
        public async Task<IActionResult> UpdateDepartment(string id, CreateDepartmentRequestDto departmentRequest)
        {
            var result = await _departmentService.UpdateDepartmentAsync(id, departmentRequest);
            return Ok(result);
        }

        [HttpPost("createdepartment")]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentRequestDto departmentRequest)
        {
            var result = await _departmentService.CreateDepartmentAsync(departmentRequest);
            return Ok(result);
        }

        // Reminder--> soft Delete
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteDepartment(string id)
        {
            var result = await _departmentService.DeleteDepartmnetAsync(id);
            return Ok(result);
        }



    }
}
