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

        // GET: api/users/detail/1
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetUserDetail(int id)
        {
            User result = await _userService.GetUserDetailAsync(id);
            return Ok(result);
        }

    }
}
