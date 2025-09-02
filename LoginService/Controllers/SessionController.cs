using LoginService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoginService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpGet]
        [Route("Validate")]
        public async Task<IActionResult> Validate([FromHeader(Name = "Authorization")] string? authHeader)
        {
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized(false);

            var token = authHeader.Substring("Bearer ".Length);

            // TODO: validate token (JWT or lookup in DB/Redis)
            bool isValid = await _sessionService.ValidateSessionAsync(token);

            if (!isValid)
                return Unauthorized(false);

            return Ok(true);
        }

        [HttpGet]
        [Route("Renew")]
        public async Task<IActionResult> Renew([FromHeader(Name = "Authorization")] string? authHeader)
        {
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized(false);

            var token = authHeader.Substring("Bearer ".Length);
            bool isRenewed = await _sessionService.RenewSessionAsync(token);

            if (!isRenewed)
                return Ok(false);

            return Ok(true);
        }
    }
}
