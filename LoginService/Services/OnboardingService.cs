using LoginService.Models;
using LoginService.Models.DTOs;
using LoginService.Models.Entities;

namespace LoginService.Services
{
    public class OnboardingService
    {
        private readonly DataContext _context;
        public OnboardingService(DataContext context)
        {
            _context = context;
        }

        public async Task InsertOnboardingLog(RegisterDTO user, int status)
        {
            OnboardingLog onboardingLog = new OnboardingLog()
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
