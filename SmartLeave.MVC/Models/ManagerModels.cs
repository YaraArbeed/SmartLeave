using System.ComponentModel.DataAnnotations;

namespace SmartLeave.MVC.Models
{
    public class PendingLeaveModel
    {
        public int Id { get; set; }
        public string LeaveType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalDays { get; set; }
        public string? Comment { get; set; }
        public string? AppliedBy { get; set; } // Employee email or name
        public DateTime AppliedOn { get; set; }
        public string? AttachmentUrl { get; set; }
    }

    public class LeaveDecisionModel
    {
        [Required]
        public int LeaveId { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        [Required(ErrorMessage = "Please provide a note for your decision")]
        [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters")]
        public string Note { get; set; } = string.Empty;
    }

    public class ManagerDashboardViewModel
    {
        public string ManagerName { get; set; } = string.Empty;
        public List<PendingLeaveModel> PendingLeaves { get; set; } = new();
        public int TotalPendingCount => PendingLeaves.Count;
    }
}
