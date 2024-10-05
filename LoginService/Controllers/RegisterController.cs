using LoginService.Models;
using LoginService.Models.DTOs;
using LoginService.Models.Entities;
using LoginService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(IUserService userService, ILogger<RegisterController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // POST: api/register/process
        [HttpPost]
        [Route("process")]
        public async Task<IActionResult> ProcessRegistration([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterUserAsync(registerDto);

            if (!result)
            {
                return BadRequest("Registration failed. Email or Username already exists.");
            }

            return Ok("User registered successfully.");
        }
    }
}
