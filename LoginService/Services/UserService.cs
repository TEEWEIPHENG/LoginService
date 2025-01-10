using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnboardingStatusEnum = LoginService.Models.Enum.OnboardingStatusEnum;
using LoginService.Helpers;
using LoginService.Data;
using LoginService.Data.Repositories;
using LoginService.Models;
using System.Reflection.Metadata.Ecma335;
using static System.Net.WebRequestMethods;

namespace LoginService.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IPasswordHasher<UserRepository> _passwordHasher;
        private readonly ILogger<UserService> _logger;
        private OnboardingService _onboardingService;
        private readonly OTPValidator _otpValidator;
        private readonly UserDetailValidator _userDetailValidator;

        public UserService(DataContext context, IPasswordHasher<UserRepository> passwordHasher, ILogger<UserService> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _onboardingService = new OnboardingService(context);
            _otpValidator = new OTPValidator(context);
            _userDetailValidator = new UserDetailValidator(context);
        }

        public async Task<OnboardingStatusEnum> RegisterAsync(RegisterModel request)
        {
            UserRepository newUser = new UserRepository();
            var status = OnboardingStatusEnum.Fail;
            try
            {
                var isValidUser = await _userDetailValidator.ValidateUsername(request.Username) 
                        || await _userDetailValidator.ValidateEmail(request.Email)
                        || await _userDetailValidator.ValidateMobileNo(request.MobileNo);

                if (isValidUser)
                {
                    status = OnboardingStatusEnum.AccountExisted;
                    _logger.LogWarning("User with this email or username already exists.");
                }
                else
                {
                    newUser.UserId = Guid.NewGuid().ToString().ToUpper();
                    newUser.Lastname = request.Lastname.Trim();
                    newUser.Firstname = request.Firstname.Trim();
                    newUser.Username = request.Username.Trim();
                    newUser.Email = request.Email.Trim();
                    newUser.MobileNo = request.MobileNo.Trim();
                    newUser.RoleId = request.RoleId.Trim();
                    newUser.CreatedAt = DateTime.Now;
                    newUser.UpdatedAt = DateTime.Now;
                    newUser.LastLogin = null;

                    newUser.Password = _passwordHasher.HashPassword(newUser, request.Password);

                    await _context.Users.AddAsync(newUser);
                    await _context.SaveChangesAsync();

                    status =OnboardingStatusEnum.Success;

                    _logger.LogInformation("User registered successfully.");
                }               
            }
            catch (Exception ex)
            {
                status = OnboardingStatusEnum.Error;
                _logger.LogError("RegisterUser Error: " + ex.Message);
            }
            finally
            {
                await _onboardingService.InsertOnboardingLog(request, (int)status);
            }

            return status;
        }

        public async Task<bool> LoginAsync(string usernameOrEmail, string password)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username.ToLower() == usernameOrEmail.Trim().ToLower() || u.Email.ToLower() == usernameOrEmail.Trim().ToLower());

                if (user != null && user.IsValid && user.IsEnabled && !user.IsLocked)
                {
                    var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
                    if (verificationResult == PasswordVerificationResult.Success)
                    {
                        // Reset WrongPasswordCount on successful login
                        user.WrongPasswordCount = 0;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        user.WrongPasswordCount++;
                         
                        const int maxAttempts = 3;
                        if (user.WrongPasswordCount >= maxAttempts)
                        {
                            user.IsLocked = true;
                        }

                        await _context.SaveChangesAsync();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Login Error: " + ex.Message);
            }

            return false;
        }
        public async Task<RequestOTPResponse> RequestOTPAsync(string username, string mobileNo)
        {
            RequestOTPResponse response = new RequestOTPResponse();
            try
            {
                _logger.LogInformation($"RequestOTP: {username}, {mobileNo}");
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username.Trim()) && u.MobileNo.Equals(mobileNo.Trim()));
                if(user != null && user.IsEnabled)
                {
                    MfaRepository mFA = new()
                    {
                        OTP = OTPGenerator.GenerateOTP(),
                        ReferenceNo = OTPGenerator.GenerateReferenceNo(),
                        UserId = user.UserId,
                        CreateAt = DateTime.Now,
                    };

                    await _context.MFA.AddAsync(mFA);
                    await _context.SaveChangesAsync();

                    response.status = true;
                    response.referenceNo = mFA.ReferenceNo;

                    //send email or sms OTP
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("RequestOTP Error: " + ex.Message);
            }
            return response;
        }

        public async Task<bool> ActivationAsync(string otp, string referenceNo)
        {
            try
            {
                var response = await _otpValidator.ValidateOTPAsync(otp.Trim(), referenceNo.Trim());
                if (response != null)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == response.UserId);
                    if (user != null)
                        user.IsValid = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Activation Error: " + ex.Message);
            }
            return false;
        }

        public async Task<string> ValidateForgotPasswordAsync(string otp, string referenceNo)
        {
            try
            {
                var response = await _otpValidator.ValidateOTPAsync(otp.Trim(), referenceNo.Trim());
                if(response != null)
                {
                    //check again user 
                    return response.UserId;
                }
                _logger.LogInformation($"ValidateForgotPassword Success: {referenceNo}");
            }
            catch (Exception ex)
            {
                _logger.LogError("ForgotPassword Error: " + ex.Message);
            }
            return null;
        }
        public async Task<bool> ForgotPasswordAsync(string newPassword, string confirmPassword, string otp, string referenceNo)
        {
            try
            {
                //validate OTP
                var response = await _otpValidator.ValidateOTPAsync(otp.Trim(), referenceNo.Trim());
                if (response != null)
                {
                    var user = await _context.Users.Where(u => u.UserId == response.UserId).FirstOrDefaultAsync();
                    if (user!= null && newPassword.Trim() == confirmPassword.Trim())
                    {
                        user.Password = _passwordHasher.HashPassword(user, newPassword.Trim());
                        //update password
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
                _logger.LogInformation($"ForgotPassword Success.");
            }
            catch (Exception ex)
            {
                _logger.LogError("ForgotPassword Error: " + ex.Message);
            }
            return false;
        }
        public async Task<bool> ResetUsernameAsync(string newUsername, string otp, string referenceNo)
        {
            try
            {
                //validate OTP
                var response = await _otpValidator.ValidateOTPAsync(otp.Trim(), referenceNo.Trim());
                if (response != null)
                {
                    var user = await _context.Users.Where(u => u.UserId == response.UserId).FirstOrDefaultAsync();
                    var isValidUsername = await _userDetailValidator.ValidateUsername(newUsername);
                    if (user != null && isValidUsername)
                    {
                        user.Username = newUsername.Trim();
                        //update password
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
                _logger.LogInformation($"ForgotPassword Success.");
            }
            catch (Exception ex)
            {
                _logger.LogError("ForgotPassword Error: " + ex.Message);
            }
            return false;
        }
    }
}
