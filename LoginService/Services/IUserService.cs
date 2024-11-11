using System.Threading.Tasks;
using LoginService.Models;
using LoginService.Models.DTOs;
using OnboardingStatusEnum = LoginService.Models.Enum.OnboardingStatus;

namespace LoginService.Services
{
    public interface IUserService
    {
        Task<OnboardingStatusEnum> RegisterAsync(RegisterDTO registerDto);
        Task<OnboardingStatusEnum> ActivationAsync(string OTP, string referenceNo);
        Task<bool> LoginAsync(string username, string password);
    }
}
