using LoginService.Models;

namespace LoginService.Services
{
    public interface IOnboardingService
    {
        Task InsertOnboardingLog(RegisterModel user, int status);
    }
}
