using LoginService.Models;
using LoginService.Models.DTOs;
using LoginService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LoginService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IUserService userService, ILogger<LoginController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [Route("process")]
        public async Task<IActionResult> ProcessLogin([FromBody] LoginDTO loginDTO)
        {
            var result = await _userService.LoginAsync(loginDTO.Username, loginDTO.Password);

            return Ok(result);
        }

    }
}
