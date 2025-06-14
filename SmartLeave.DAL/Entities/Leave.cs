using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.DAL.Entities
{
    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class Leave
    {
        /// <summary>
        /// Primary key for Leave (int).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The employee who applied—FK to AspNetUsers(Id).
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Navigation to the Employee (User).
        /// </summary>
        public virtual User Employee { get; set; }

        /// <summary>
        /// The type of leave—FK to LeaveTypes(Id).
        /// </summary>
        public int LeaveTypeId { get; set; }

        /// <summary>
        /// Navigation to the LeaveType.
        /// </summary>
        public virtual LeaveType LeaveType { get; set; }

        /// <summary>
        /// First day of requested leave.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Last day of requested leave (inclusive).
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// When the employee submitted the request.
        /// </summary>
        public DateTime AppliedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Pending / Approved / Rejected.
        /// </summary>
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        /// <summary>
        /// The manager or admin who approved/rejected—FK to AspNetUsers(Id).
        /// Null if still pending.
        /// </summary>
        public string? ApprovedById { get; set; } 

        /// <summary>
        /// Navigation to the approver (User).
        /// </summary>
        public virtual User? ApprovedBy { get; set; }

        /// <summary>
        /// When it was approved or rejected. Null if still pending.
        /// </summary>
        public DateTime? DecisionDate { get; set; }

        /// <summary>
        /// Employee’s reason for taking leave.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Manager/Admin’s note on approval/rejection.
        /// </summary>
        public string? ApprovalNote { get; set; }

        /// <summary>
        /// Optional link to a doctor’s note or other document.
        /// </summary>
        public string? AttachmentUrl { get; set; }

        /// <summary>
        /// Calculated as (EndDate – StartDate).Days + 1 (decimal for partial days if needed).
        /// </summary>
        public decimal TotalDays { get; set; }

    }
}
