namespace LoginService.Data.Entities
{
    public class UserSession
    {
        public int Id { get; set; }
        public string SessionHash { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public bool IsRevoked { get; set; } = false;
    }
}
