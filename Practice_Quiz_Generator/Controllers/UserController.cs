using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practice_Quiz_Generator.Application.Services.Interfaces;

namespace Practice_Quiz_Generator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllUser()
        //{
        //    //var result = await _userService);
        //    //return Ok(result);
        //}


        [HttpGet("email")]
        public async Task<IActionResult> GetUserByName(string email)
        {
            var result = await _userService.GetUserByEmailAsync(email);
            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            return Ok(result);
        }


    }
}
