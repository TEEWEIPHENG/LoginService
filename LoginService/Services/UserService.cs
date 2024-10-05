using LoginService.Models.DTOs;
using LoginService.Models;
using Microsoft.AspNetCore.Identity;
using LoginService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoginService.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<UserService> _logger;

        public UserService(DataContext context, IPasswordHasher<User> passwordHasher, ILogger<UserService> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<bool> RegisterUserAsync(RegisterDTO registerDto)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email || u.Username == registerDto.Username);
                if (existingUser != null)
                {
                    _logger.LogWarning("User with this email or username already exists.");
                }

                // Create new user
                var newUser = new User
                {
                    UserId = Guid.NewGuid().ToString(),
                    Lastname = registerDto.Lastname,
                    Firstname = registerDto.Firstname,
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    MobileNo = registerDto.MobileNo,
                    RoleId = "test role", //waiting
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    LastLogin = null
                };

                // Hash the password
                newUser.Password = _passwordHasher.HashPassword(newUser, registerDto.Password);

                // Add the new user to the database
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User registered successfully.");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("RegisterUser Error: ", ex.Message);
                return false;
            }
        }
    }
}
