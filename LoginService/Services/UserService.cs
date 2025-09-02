using Azure.Core;
using LoginService.Data.Entities;
using LoginService.Data.Repositories;
using LoginService.Helpers;
using LoginService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LoginService.Services
{
    public class UserService : IUserService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<UserService> _logger;

        public UserService(ISessionRepository sessionRepository, IUserRepository userRepository, IPasswordHasher<User> passwordHasher, ILogger<UserService> logger, IRoleRepository roleRepository)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _roleRepository = roleRepository;
        }

        public async Task<ProcessRegisterResponse> RegisterAsync(RegisterModel request)
        {
            try
            {
                _logger.LogInformation("========== Register Start ==========");
                _logger.LogInformation("Username: {Username}, Lastname: {Lastname}, Firstname: {Firstname}, Mobile: {Mobile}, Email: {Email}", 
                    request.Username, request.Lastname, request.Firstname, request.MobileNo, request.Email);

                if (request == null)
                {
                    return new ProcessRegisterResponse
                    {
                        success = false,
                        message = "Invalid user details."
                    };
                }

                // Validate the request
                var isDuplicate = await _userRepository.ExistsAsync(request.Username, request.MobileNo, request.Email);
                if (isDuplicate)
                {
                    return new ProcessRegisterResponse
                    {
                        success = false,
                        message = "Username, Email, or Mobile number already exists."
                    };
                }

                var user = new User
                {
                    Firstname = request.Firstname.Trim(),
                    Lastname = request.Lastname.Trim(),
                    Username = request.Username.Trim(),
                    Email = request.Email.Trim(),
                    MobileNo = request.MobileNo.Trim(),
                    RoleId = string.IsNullOrWhiteSpace(request.Role) ? await _roleRepository.GetRoleIdByNameAsync("User") : await _roleRepository.GetRoleIdByNameAsync(request.Role),
                    Password = _passwordHasher.HashPassword(new User(), request.Password),
                    IsValid = true,
                };

                await _userRepository.AddAsync(user);

                return new ProcessRegisterResponse
                {
                    success = true,
                    message = "User registered successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("RegisterUser Error: " + ex.Message);
                return new ProcessRegisterResponse
                {
                    success = false,
                    message = "An error occurred while registering the user."
                };
            }
            finally
            {
                _logger.LogInformation("========== Register End ==========");
            }
        }

        public async Task<ProcessLoginResponse> LoginAsync(string usernameOrEmail, string password, string ipAddress, string userAgent)
        {
            _logger.LogInformation("========== Login Start ==========");
            _logger.LogInformation("Username OR Email: {usernameOrEmail}", usernameOrEmail);

            ProcessLoginResponse response = new ProcessLoginResponse()
            {
                success = false,
                message = "Login failed. Incorrect username or password."
            };

            try
            {
                var user = await _userRepository.GetByUsernameOrEmailAsync(usernameOrEmail);

                if (user != null && user.IsValid && user.IsEnabled && !user.IsLocked)
                {
                    // Verify the password
                    var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
                    if (verificationResult == PasswordVerificationResult.Success)
                    {
                        // Reset WrongPasswordCount on successful login
                        user.WrongPasswordCount = 0;
                        await _userRepository.UpdateAsync(user);

                        //generate session token
                        var token = SessionHelper.GenerateToken(32);
                        var hash = SessionHelper.HashToken(token);

                        //store inside db
                        var session = new UserSession
                        {
                            SessionHash = hash,
                            UserId = user.UserId,
                            CreatedAt = DateTime.UtcNow,
                            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                            IpAddress = ipAddress,
                            UserAgent = userAgent
                        };
                        await _sessionRepository.CreateAsync(session);

                        response.sessionToken = token;
                        response.expiresAt = session.ExpiresAt;
                        response.success = true;
                        response.message = "Login successful.";
                    }
                    else
                    {
                        user.WrongPasswordCount++;

                        const int maxAttempts = 3;
                        if (user.WrongPasswordCount >= maxAttempts)
                        {
                            user.IsLocked = true;
                        }

                        await _userRepository.UpdateAsync(user);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Login Error: " + ex.Message);
            }
            finally
            {
                _logger.LogInformation("========== Login End ==========");
            }

            return response;
        }
        public async Task<RequestOTPResponse> RequestOTPAsync(string username, string mobileNo)
        {
            RequestOTPResponse response = new RequestOTPResponse();
            try
            {
                _logger.LogInformation($"RequestOTP: {username}, {mobileNo}");
                //var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username.Trim()) && u.MobileNo.Equals(mobileNo.Trim()));
                //if(user != null && user.IsEnabled)
                //{
                //    MfaRepository mFA = new()
                //    {
                //        OTP = OTPGenerator.GenerateOTP(),
                //        ReferenceNo = OTPGenerator.GenerateReferenceNo(),
                //        UserId = user.UserId,
                //        CreateAt = DateTime.Now,
                //    };

                //    await _context.MFA.AddAsync(mFA);
                //    await _context.SaveChangesAsync();

                //    response.status = true;
                //    response.referenceNo = mFA.ReferenceNo;

                //    //send email or sms OTP
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("RequestOTP Error: " + ex.Message);
            }
            finally {
                //insert log for request OTP
            }
            return response;
        }

        public async Task<bool> ActivationAsync(string otp, string referenceNo)
        {
            try
            {
                //var response = await _otpValidator.ValidateOTPAsync(otp.Trim(), referenceNo.Trim());
                //if (response != null)
                //{
                //    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == response.UserId);
                //    if (user != null)
                //        user.IsValid = true;
                //    await _context.SaveChangesAsync();
                //    return true;
                //}
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
                //var response = await _otpValidator.ValidateOTPAsync(otp.Trim(), referenceNo.Trim());
                //if(response != null)
                //{
                //    //check again user 
                //    return response.UserId;
                //}
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
                //var response = await _otpValidator.ValidateOTPAsync(otp.Trim(), referenceNo.Trim());
                //if (response != null)
                //{
                //    var user = await _context.Users.Where(u => u.UserId == response.UserId).FirstOrDefaultAsync();
                //    if (user!= null && newPassword.Trim() == confirmPassword.Trim())
                //    {
                //        user.Password = _passwordHasher.HashPassword(user, newPassword.Trim());
                //        //update password
                //        await _context.SaveChangesAsync();
                //        return true;
                //    }
                //}
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
                //var response = await _otpValidator.ValidateOTPAsync(otp.Trim(), referenceNo.Trim());
                //if (response != null)
                //{
                //    var user = await _context.Users.Where(u => u.UserId == response.UserId).FirstOrDefaultAsync();
                //    var isValidUsername = await _userDetailValidator.ValidateUsername(newUsername);
                //    if (user != null && isValidUsername)
                //    {
                //        user.Username = newUsername.Trim();
                //        //update password
                //        await _context.SaveChangesAsync();
                //        return true;
                //    }
                //}
                _logger.LogInformation($"ForgotPassword Success.");
            }
            catch (Exception ex)
            {
                _logger.LogError("ForgotPassword Error: " + ex.Message);
            }
            return false;
        }

        public async Task<bool> Logout(string token)
        {
            _logger.LogInformation("========== Logout Start ==========");
            try
            {
                var hashToken = SessionHelper.HashToken(token);
                var session = await _sessionRepository.GetByHashAsync(hashToken);
                await _sessionRepository.RevokeAsync(session);
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
