namespace SmartLeave.API.DTOs
{
    public class CompleteProfileDto
    {
        public int? OfficeId { get; set; }
        public int? CountryId { get; set; }
        public int? TeamId { get; set; } // For employees
        public string? ManagerId { get; set; } // For employees
        public string? Position { get; set; }
        public DateTime? DateHired { get; set; }
    }
}
