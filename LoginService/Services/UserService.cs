using LoginService.Models.DTOs;
using LoginService.Models;
using Microsoft.AspNetCore.Identity;
using LoginService.Models.Entities;
using Microsoft.EntityFrameworkCore;
using OnboardingStatusEnum = LoginService.Models.Enum.OnboardingStatus;
using Microsoft.AspNetCore.Components.Forms;
using LoginService.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LoginService.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<UserService> _logger;
        private OnboardingService _onboardingService;

        public UserService(DataContext context, IPasswordHasher<User> passwordHasher, ILogger<UserService> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _onboardingService = new OnboardingService(context);
        }

        public async Task<OnboardingStatusEnum> RegisterAsync(RegisterDTO registerDto)
        {
            User newUser = new User();
            var status = OnboardingStatusEnum.Fail;
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email.Trim() || u.Username == registerDto.Username.Trim() || u.MobileNo == registerDto.MobileNo.Trim());
                if (existingUser != null)
                {
                    status = OnboardingStatusEnum.AccountExisted;
                    _logger.LogWarning("User with this email or username already exists.");
                }
                else
                {
                    newUser.UserId = Guid.NewGuid().ToString().ToUpper();
                    newUser.Lastname = registerDto.Lastname.Trim();
                    newUser.Firstname = registerDto.Firstname.Trim();
                    newUser.Username = registerDto.Username.Trim();
                    newUser.Email = registerDto.Email.Trim();
                    newUser.MobileNo = registerDto.MobileNo.Trim();
                    newUser.RoleId = registerDto.RoleId.Trim();
                    newUser.CreatedAt = DateTime.Now;
                    newUser.UpdatedAt = DateTime.Now;
                    newUser.LastLogin = null;

                    newUser.Password = _passwordHasher.HashPassword(newUser, registerDto.Password);

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
                await _onboardingService.InsertOnboardingLog(registerDto, (int)status);
            }

            return status;
        }

        public async Task<bool> LoginAsync(string usernameOrEmail, string password)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == usernameOrEmail.Trim() || u.Email == usernameOrEmail.Trim());

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
        public async Task<bool> ValidateActivationAsync(string username, string mobileNo)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username.Trim()) && u.MobileNo.Equals(mobileNo.Trim()));
                if(user != null && user.IsEnabled)
                {
                    if (user.IsValid)
                    {
                        return true;
                    }
                    else
                    {
                        MFA mFA = new()
                        {
                            OTP = OTPGenerator.GenerateOTP(),
                            ReferenceNo = OTPGenerator.GenerateReferenceNo(),
                            UserId = user.UserId,
                            CreateAt = DateTime.Now,
                        };
                        //send OTP
                        await _context.MFA.AddAsync(mFA);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Activation Error: " + ex.Message);
            }
            return false;
        }

        public async Task<bool> ActivationAsync(string otp, string referenceNo)
        {
            try
            {
                return await _context.MFA.AnyAsync(m => m.OTP == otp.Trim() && m.OTP == referenceNo.Trim() && DateTime.Now < m.CreateAt.AddMinutes(2));
            }
            catch (Exception ex)
            {
                _logger.LogError("Activation Error: " + ex.Message);
            }
            return false;
        }
    }
}
