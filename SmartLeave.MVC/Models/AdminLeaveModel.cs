using System;
using System.Text.Json.Serialization;

namespace SmartLeave.MVC.Models
{
    public class AdminLeaveModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("leaveType")]
        public string LeaveType { get; set; } = string.Empty;

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("totalDays")]
        public decimal TotalDays { get; set; }

        [JsonPropertyName("comment")]
        public string? Comment { get; set; }

        [JsonPropertyName("appliedBy")]
        public string? AppliedBy { get; set; }

        [JsonPropertyName("employeeId")]
        public string? EmployeeId { get; set; }

        [JsonPropertyName("appliedOn")]
        public DateTime AppliedOn { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("managerName")]
        public string? ManagerName { get; set; }

        [JsonPropertyName("team")]
        public string? Team { get; set; }

        [JsonPropertyName("office")]
        public string? Office { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }
    }
}
