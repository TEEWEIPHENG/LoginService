namespace LoginService.Services
{
    public interface ISessionService
    {
        public Task<bool> ValidateSessionAsync(string sessionToken);
        public Task<bool> RenewSessionAsync(string sessionToken);
    }
}
