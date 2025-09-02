using LoginService.Data.Entities;
using LoginService.Helpers;
using LoginService.Models;
using LoginService.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
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

            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            string userAgent = Request.Headers["User-Agent"].ToString();

            var result = await _userService.LoginAsync(request.Username, request.Password, ipAddress, userAgent);

            //if (result.success)
            //{
            //    Response.Cookies.Append("auth_session", result.sessionToken, new CookieOptions
            //    {
            //        HttpOnly = true,
            //        Secure = false, //https only
            //        SameSite = SameSiteMode.None,
            //        Domain = "localhost:5000",          // so cookie works across services
            //        Expires = result.expiresAt,
            //    });
            //}
            return Ok(result);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> LogoutAsync([FromHeader(Name = "Authorization")] string? authHeader)
        {
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized(false);

            var token = authHeader.Substring("Bearer ".Length);
            Response.Cookies.Delete("auth_session");
            var ok = await _userService.Logout(token);
            return Ok(ok);
        }

        //[HttpPost]
        //[Route("RequestOTP")]
        //public async Task<IActionResult> RequestOTP([FromBody] RequestOTPModel request)
        //{
        //    var result = await _userService.RequestOTPAsync(request.username, request.mobileNo);

        //    return Ok(result);
        //}

        //[HttpPost]
        //[Route("ForgotPassword")]
        //public async Task<IActionResult> ForgotPassword([FromBody] ResetPasswordModel request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var result = await _userService.ForgotPasswordAsync(request.newPassword.Trim(), request.confirmPassword.Trim(), request.otp.Trim(), request.referenceNo.Trim());

        //    return result ? Ok("Activated Successful") : Ok("Activation Failure");
        //}

        //[HttpPost]
        //[Route("ResetUsername")]
        //public async Task<IActionResult> ResetUsername([FromBody] ResetUsernameModel request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var result = await _userService.ResetUsernameAsync(request.newUsername.Trim(), request.otp.Trim(), request.referenceNo.Trim());

        //    return Ok(result);
        //}
    }
}
