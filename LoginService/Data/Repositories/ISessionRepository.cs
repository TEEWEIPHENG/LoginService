using LoginService.Data.Entities;

namespace LoginService.Data.Repositories
{
    public interface ISessionRepository
    {
        public Task<Session?> GetByTokenAsync(string token);
        public Task AddAsync(Session session);
        public Task UpdateAsync(Session session);
        public Task<Session?> GetByUserIdAsync(string userId);
        public Task<bool> InvalidateSessionAsync(string userId);
        public Task RefreshTokenAsync(string token, DateTime newExpireAt);
    }
}
