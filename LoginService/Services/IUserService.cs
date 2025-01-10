using System.Threading.Tasks;
using LoginService.Models;
using OnboardingStatusEnum = LoginService.Models.Enum.OnboardingStatusEnum;

namespace LoginService.Services
{
    public interface IUserService
    {
        Task<OnboardingStatusEnum> RegisterAsync(RegisterModel registerDto);
        Task<RequestOTPResponse> RequestOTPAsync(string username, string mobileNo);
        Task<bool> ActivationAsync(string otp, string referenceNo);
        Task<bool> LoginAsync(string username, string password);
        Task<bool> ForgotPasswordAsync(string newPassword, string confirmPassword, string otp, string referenceNo);
        Task<bool> ResetUsernameAsync(string newUsername, string otp, string referenceNo);
    }
}
