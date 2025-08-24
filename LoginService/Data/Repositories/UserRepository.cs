using LoginService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoginService.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByMobileNoAsync(string mobileNo)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.MobileNo == mobileNo);
        }

        public async Task<bool> ExistsAsync(string username, string mobileNo, string email)
        {
            return await _context.Users.AnyAsync(u =>
                u.Username == username ||
                u.MobileNo == mobileNo ||
                u.Email == email);
        }
    }
}
