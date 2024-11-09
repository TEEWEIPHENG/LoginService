using System.ComponentModel.DataAnnotations;

namespace LoginService.Models
{
    public class OnboardingStatus
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
