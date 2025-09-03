using Azure.Core;
using LoginService.Data.Entities;
using LoginService.Data.Repositories;
using LoginService.Helpers;
using LoginService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginService.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _config;

        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ISessionRepository _sessionRepository;

        public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, ILogger<UserService> logger, IRoleRepository roleRepository, IConfiguration config, ISessionRepository sessionRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _roleRepository = roleRepository;
            _config = config;
            _sessionRepository = sessionRepository;
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

        public async Task<ProcessLoginResponse> LoginAsync(string usernameOrEmail, string password)
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

                        var tokenString = await GenerateJwtTokenAsync(user);

                        response.sessionToken = tokenString;
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

        public async Task<bool> Logout(string token)
        {
            _logger.LogInformation("========== Logout Start ==========");
            try
            {
                return await _sessionRepository.InvalidateSessionAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<string> RefreshJwtToken(string token)
        {
            _logger.LogInformation("========== Refresh Token Start ==========");
            var session = _sessionRepository.GetByTokenAsync(token);
            var user = await _userRepository.GetByUserIdAsync(session.Result.UserId);

            if (session != null && user != null)
            {
                return await GenerateJwtTokenAsync(user);
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid session or user.");
            }
        }

        private async Task<string> GenerateJwtTokenAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured"));
            var expireMinutes = int.Parse(_config["Jwt:ExpireMinutes"] ?? "60");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.RoleId)
                }),
                Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            Session session = new Session
            {
                SessionId = Guid.NewGuid().ToString(),
                UserId = user.UserId,
                Token = tokenString,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddMinutes(expireMinutes),
                IsActive = true
            };
            await _sessionRepository.AddAsync(session);

            return tokenString;
        }
    }
}
