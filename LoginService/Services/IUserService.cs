using System.Threading.Tasks;
using LoginService.Models;
using LoginService.Models.DTOs;
using OnboardingStatusEnum = LoginService.Models.Enum.OnboardingStatus;

namespace LoginService.Services
{
    public interface IUserService
    {
        Task<OnboardingStatusEnum> RegisterUserAsync(RegisterDTO registerDto);
        Task<User> GetUserDetailAsync(int Id);
    }
}
