using System.ComponentModel.DataAnnotations;

namespace LoginService.Data.Repositories
{
    public class OnboardingStatusRepository
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
