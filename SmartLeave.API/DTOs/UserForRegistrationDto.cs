using System.ComponentModel.DataAnnotations;

namespace SmartLeave.API.DTOs
{
    public class UserForRegistrationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Position { get; set; }

        /// <summary>
        /// FK to Offices table
        /// </summary>
        public int OfficeId { get; set; }

        /// <summary>
        /// FK to Countries table
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// When they were hired
        /// </summary>
        public DateTime DateHired { get; set; }

        public string Role { get; set; }  // "Admin", "Manager", "Employee"
    }
}
