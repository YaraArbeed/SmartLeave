using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartLeave.MVC.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum LeaveStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
    public class LeaveResponseModel
    {
        public int Id { get; set; }
        public string LeaveType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveStatus Status { get; set; } 
        public string? ManagerName { get; set; } = string.Empty;
        public string? Comment { get; set; }
        public decimal TotalDays { get; set; }
    }

    public class LeaveBalanceModel
    {
        public string LeaveType { get; set; } = string.Empty;
        public decimal UsedDays { get; set; }
        public decimal RemainingDays { get; set; }
        public decimal Allowance { get; set; }
    }

    public class LeaveRequestModel
    {
        [Required]
        public int LeaveTypeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string? Comment { get; set; }

        public IFormFile? Attachment { get; set; }
    }

    public class PublicHolidayModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime HolidayDate { get; set; }
        public string? Description { get; set; }
    }


}
