using LoginService.Data.Entities;
using LoginService.Helpers;
using LoginService.Models;
using LoginService.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

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

        [HttpGet]
        [Route("Ping")]
        public IActionResult Ping()
        {
            return Ok(true);
        }

        [HttpPost]
        [Route("Process")]
        public async Task<IActionResult> ProcessLogin([FromBody] LoginModel request)
        {
            var result = await _userService.LoginAsync(request.Username, request.Password);

            return Ok(result);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> LogoutAsync([FromHeader(Name = "Authorization")] string? authHeader)
        {
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized(false);

            var token = authHeader.Substring("Bearer ".Length);
            var ok = await _userService.Logout(token);
            return Ok(ok);
        }

        [HttpPost("RefreshToken")]
        public IActionResult Refresh([FromHeader(Name = "Authorization")] string? authHeader)
        {
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized(false);

            var token = authHeader.Substring("Bearer ".Length);
            var newJwt = _userService.RefreshJwtToken(token);

            return Ok(new { token = newJwt });
        }
    }
}
