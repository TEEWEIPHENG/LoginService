using LoginService.Models;
using LoginService.Services;
using Microsoft.AspNetCore.Identity.Data;
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
        [Route("Process")]
        public async Task<IActionResult> ProcessLogin([FromBody] LoginModel request)
        {
            var result = await _userService.LoginAsync(request.Username, request.Password);

            return Ok(result);
        }

        [HttpPost]
        [Route("RequestOTP")]
        public async Task<IActionResult> RequestOTP([FromBody] RequestOTPModel request)
        {
            var result = await _userService.RequestOTPAsync(request.username, request.mobileNo);

            return Ok(result);
        }
        
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ResetPasswordModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.ForgotPasswordAsync(request.newPassword.Trim(), request.confirmPassword.Trim(), request.otp.Trim(), request.referenceNo.Trim());

            return result ? Ok("Activated Successful") : Ok("Activation Failure");
        }

        [HttpPost]
        [Route("ResetUsername")]
        public async Task<IActionResult> ResetUsername([FromBody] ResetUsernameModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.ResetUsernameAsync(request.newUsername.Trim(), request.otp.Trim(), request.referenceNo.Trim());

            return Ok(result);
        }
    }
}
