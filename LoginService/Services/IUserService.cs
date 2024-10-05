using System.Threading.Tasks;
using LoginService.Models.DTOs;

namespace LoginService.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterDTO registerDto);
    }
}
