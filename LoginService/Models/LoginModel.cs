namespace LoginService.Models
{
    public class LoginModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class ProcessLoginResponse : CommonApiResponse
    {
        public string sessionToken { get; set; }
    }
}
