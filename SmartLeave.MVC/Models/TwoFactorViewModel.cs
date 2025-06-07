using System.ComponentModel.DataAnnotations;

namespace SmartLeave.MVC.Models
{
    public class TwoFactorViewModel
    {
        public string? Email { get; set; }

        [Required]
        public string? Token { get; set; }

        public string Provider { get; set; } = "Email";
    }
}
