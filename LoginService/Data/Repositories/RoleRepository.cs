using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LoginService.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {

        private readonly DataContext _context;

        public RoleRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<string> GetRoleIdByNameAsync(string roleName)
        {
            return await _context.Roles.Where(r => r.Name == roleName)
                .Select(r => r.RoleId)
                .FirstOrDefaultAsync() ?? throw new ValidationException($"Role '{roleName}' not found.");
        }

    }
}
