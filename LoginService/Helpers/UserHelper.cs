using Azure;
using LoginService.Data;
using LoginService.Data.Repositories;
using LoginService.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginService.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly IUserRepository _userRepository;
        public UserHelper(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
