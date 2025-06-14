namespace SmartLeave.Common.DTOs.Employee
{
    public class LeaveResponseDto
    {
        public int Id { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string? ManagerName { get; set; }
        public string? Comment { get; set; }
        public decimal TotalDays { get; set; }
        public string AppliedBy { get; set; } = string.Empty;
        public DateTime AppliedOn { get; set; }

        public string? AttachmentUrl { get; set; } // if you store a file URL

    }
}
