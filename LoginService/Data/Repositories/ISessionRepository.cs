using LoginService.Data.Entities;

namespace LoginService.Data.Repositories
{
    public interface ISessionRepository
    {
        Task CreateAsync(UserSession session);
        Task<UserSession?> GetByHashAsync(string sessionHash);
        Task RevokeAsync(UserSession session);
        Task CleanupExpiredAsync();
    }

}
