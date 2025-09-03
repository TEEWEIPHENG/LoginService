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
        public async Task<User?> GetByUserIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId.ToLower() == userId.Trim().ToLower());
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.Trim().ToLower());
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.Trim().ToLower());
        }

        public async Task<User?> GetByMobileNoAsync(string mobileNo)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.MobileNo == mobileNo);
        }

        public async Task<User?> GetByUsernameOrEmailAsync(string input)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == input.Trim().ToLower() || u.Email.ToLower() == input.Trim().ToLower());
        }

        public async Task<bool> ExistsAsync(string username, string mobileNo, string email)
        {
            return await _context.Users.AnyAsync(u =>
                u.Username == username ||
                u.MobileNo == mobileNo ||
                u.Email == email);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
