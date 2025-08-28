using System.Threading.Tasks;
using LoginService.Models;

namespace LoginService.Services
{
    public interface IUserService
    {
        Task<ProcessRegisterResponse> RegisterAsync(RegisterModel registerDto);
        Task<RequestOTPResponse> RequestOTPAsync(string username, string mobileNo);
        Task<bool> ActivationAsync(string otp, string referenceNo);
        Task<ProcessLoginResponse> LoginAsync(string username, string password, string ipAddress, string userAgent);
        Task<bool> ForgotPasswordAsync(string newPassword, string confirmPassword, string otp, string referenceNo);
        Task<bool> ResetUsernameAsync(string newUsername, string otp, string referenceNo);
        Task<bool> SessionAuthentication(string token);
        Task<bool> Logout(string token);
    }
}
