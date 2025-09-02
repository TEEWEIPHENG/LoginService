using LoginService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoginService.Data.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DataContext _ctx;
        public SessionRepository(DataContext ctx) => _ctx = ctx;

        public async Task CreateAsync(UserSession session)
        {
            await _ctx.UserSession.AddAsync(session);
            await _ctx.SaveChangesAsync();
        }

        public Task<UserSession?> GetByHashAsync(string sessionHash)
        {
            return _ctx.UserSession.FirstOrDefaultAsync(s => s.SessionHash == sessionHash && !s.IsRevoked && s.ExpiresAt > DateTime.UtcNow);
        }

        public async Task RevokeAsync(UserSession session)
        {
            session.IsRevoked = true;
            _ctx.UserSession.Update(session);
            await _ctx.SaveChangesAsync();
        }

        public async Task CleanupExpiredAsync()
        {
            var expired = await _ctx.UserSession.Where(s => s.ExpiresAt <= DateTime.UtcNow || s.IsRevoked).ToListAsync();
            if (expired.Any())
            {
                _ctx.UserSession.RemoveRange(expired);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(UserSession session)
        {
            _ctx.UserSession.Update(session);
            await _ctx.SaveChangesAsync();
        }
    }

}
