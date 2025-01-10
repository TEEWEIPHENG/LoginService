using Azure;
using LoginService.Data;
using LoginService.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LoginService.Helpers
{
    public class OTPValidator(DataContext context)
    {
        private readonly DataContext _context = context;

        public async Task<MfaRepository?> ValidateOTPAsync(string otp, string referenceNo)
        {
            var result = await _context.MFA.FirstOrDefaultAsync(m => m.OTP.Equals(otp) && m.ReferenceNo.Equals(referenceNo) && m.IsValid == 0);
            if (result != null && DateTime.Now < result.CreateAt.AddMinutes(2))
            {
                result.IsValid = 1;
                await _context.SaveChangesAsync();

                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
