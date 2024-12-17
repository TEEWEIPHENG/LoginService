using LoginService.Data;
using LoginService.Data.Repositories;
using LoginService.Models;

namespace LoginService.Services
{
    public class OnboardingService : IOnboardingService
    {
        private readonly DataContext _context;
        public OnboardingService(DataContext context)
        {
            _context = context;
        }

        public async Task InsertOnboardingLog(RegisterModel user, int status)
        {
            OnboardingLogRepository onboardingLog = new OnboardingLogRepository()
            {
                Lastname = user.Lastname,
                Firstname = user.Firstname,
                Username = user.Username,
                MobileNo = user.MobileNo,
                Email = user.Email,
                Status = status,
                CreatedAt = DateTime.Now,
            };
            await _context.OnboardingLogs.AddAsync(onboardingLog);
            await _context.SaveChangesAsync();
        }
    }
}
