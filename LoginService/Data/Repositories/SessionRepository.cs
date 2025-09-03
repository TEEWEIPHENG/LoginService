using LoginService.Data.Entities;

namespace LoginService.Data.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DataContext _context;

        public SessionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Session session)
        {
            await _context.AddAsync(session);
            await _context.SaveChangesAsync();
        }

        public Task<Session?> GetByTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Session session)
        {
            throw new NotImplementedException();
        }

        public Task<Session?> GetByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InvalidateSessionAsync(string token)
        {
            var session = _context.Sessions.FirstOrDefault(s => s.Token == token);
            session.IsActive = false;
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task RefreshTokenAsync(string token, DateTime newExpireAt)
        {
            throw new NotImplementedException();
        }
    }
}
