namespace LoginService.Data.Repositories
{
    public interface IRoleRepository
    {
        Task<string> GetRoleIdByNameAsync(string roleName);
    }
}
