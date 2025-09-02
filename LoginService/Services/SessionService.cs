using LoginService.Data.Repositories;
using LoginService.Helpers;

namespace LoginService.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly ILogger<SessionService> _logger;
        public SessionService(ISessionRepository sessionRepository, ILogger<SessionService> logger)
        {
            _sessionRepository = sessionRepository;
            _logger = logger;
        }

        public async Task<bool> ValidateSessionAsync(string token)
        {
            _logger.LogInformation("========== Validate Session Start ==========");
            try
            {
                var hashToken = SessionHelper.HashToken(token);

                var session = await _sessionRepository.GetByHashAsync(hashToken);

                if (session != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> RenewSessionAsync(string token)
        {
            _logger.LogInformation("========== Session Renewal Start ==========");
            try
            {
                var hashToken = SessionHelper.HashToken(token);
                var session = await _sessionRepository.GetByHashAsync(hashToken);
                if (session != null)
                {
                    session.ExpiresAt = DateTime.UtcNow.AddMinutes(5);
                    await _sessionRepository.UpdateAsync(session);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
