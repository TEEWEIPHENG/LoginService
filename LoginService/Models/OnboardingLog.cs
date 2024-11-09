using System.ComponentModel.DataAnnotations;

namespace LoginService.Models
{
    public class OnboardingLog
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Lastname { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty; 
        [Required]
        [MaxLength(255)]
        public string MobileNo { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public int Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
