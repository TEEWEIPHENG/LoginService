using LoginService.Models;
using LoginService.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        [Route("Process")]
        public async Task<IActionResult> ProcessRegistration([FromBody] RegisterModel registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterAsync(registerDto);
            
            return Ok(result.GetDescription());
        }

        [HttpPost]
        [Route("RequestOTP")]
        public async Task<IActionResult> RequestOTP([FromBody] RequestOTPModel request)
        {
            var result = await _userService.RequestOTPAsync(request.username, request.mobileNo);

            return Ok(result);
        }

        [HttpPost]
        [Route("Activate")]
        public async Task<IActionResult> ActivateUser([FromBody] MfaModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.ActivationAsync(request.otp, request.referenceNo);

            return result ? Ok("Activated Successful") : Ok("Activation Failure");
        }
    }
}
