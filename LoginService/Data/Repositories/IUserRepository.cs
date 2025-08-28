using LoginService.Data.Entities;

namespace LoginService.Data.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByMobileNoAsync(string mobileNo);
        Task<User?> GetByUsernameOrEmailAsync(string input);
        Task<bool> ExistsAsync(string username, string mobileNo, string email);
    }

}
