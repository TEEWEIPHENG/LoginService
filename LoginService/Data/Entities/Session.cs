using System.ComponentModel.DataAnnotations;

namespace LoginService.Data.Entities
{
    public class Session
    {
        [Key]
        public required string SessionId { get; set; }
        [Required]
        public required string Token { get; set; }
        [Required]
        public required string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ExpireAt { get; set; }
        public bool IsActive { get; set; }
    }
}
