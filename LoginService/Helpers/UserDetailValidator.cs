using LoginService.Data;
using Microsoft.EntityFrameworkCore;

namespace LoginService.Helpers
{
    public class UserDetailValidator
    {
        private readonly DataContext _dataContext;
        public UserDetailValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> ValidateUsername(string username)
        {
            return await _dataContext.Users.AnyAsync(u => u.Username.ToLower() == username.Trim().ToLower());
        }

        public async Task<bool> ValidateEmail(string email)
        {
            return await _dataContext.Users.AnyAsync(u => u.Email.ToLower() == email.Trim().ToLower());
        }

        public async Task<bool> ValidateMobileNo(string mobileNo)
        {
            return await _dataContext.Users.AnyAsync(u => u.MobileNo.ToLower() == mobileNo.Trim().ToLower());
        }
    }
}
