using System.ComponentModel.DataAnnotations;

namespace SmartLeave.API.DTOs
{
    public class TwoFactorDto
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Provider { get; set; }
        public string? Token { get; set; }
    }
}
