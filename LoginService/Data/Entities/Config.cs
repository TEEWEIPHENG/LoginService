using System.ComponentModel.DataAnnotations;

namespace LoginService.Data.Entities
{
    public class Config
    {
        [Key]
        public int Id { get; set; }
        public string ConfigKey { get; set; } = string.Empty;
        public string ConfigValue { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
        public string OpenToRoles { get; set; } = string.Empty;

    }
}
