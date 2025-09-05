using System.Threading.Tasks;
using LoginService.Data.Entities;
using LoginService.Models;

namespace LoginService.Services
{
    public interface IUserService
    {
        Task<ProcessRegisterResponse> RegisterAsync(RegisterModel registerDto);
        Task<ProcessLoginResponse> LoginAsync(string username, string password);
        Task<bool> Logout(string token);
        Task<User> GetUserInfo(string userId);
    }
}
