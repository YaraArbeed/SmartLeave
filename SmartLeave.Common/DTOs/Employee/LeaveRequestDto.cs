using Microsoft.AspNetCore.Http;

namespace SmartLeave.Common.DTOs.Employee
{
    public class LeaveRequestDto
    {
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Comment { get; set; }
        public IFormFile? Attachment { get; set; }    
        public string? ApprovalNote { get; set; }
        public string? ApprovedById { get; set; }

    }
}
