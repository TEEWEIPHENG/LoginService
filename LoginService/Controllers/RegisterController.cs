using LoginService.Models.DTOs;
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
        public async Task<IActionResult> ProcessRegistration([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterAsync(registerDto);
            
            return Ok(result.GetDescription());
        }

        [HttpGet]
        [Route("ValidateActivation")]
        public async Task<IActionResult> ValidateActivationAsync([FromBody] ValidateActivationDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.ValidateActivationAsync(request.Username, request.MobileNo);

            return result ? Ok(result) : BadRequest(ModelState);
        }

        [HttpPost]
        [Route("Activate")]
        public async Task<IActionResult> ActivateUser([FromBody] MfaDTO request)
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
