using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.DAL.Entities
{
    public class LeaveBalance
    {
        // <summary>
        /// Primary key for LeaveBalance (int).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The employee whose balance this is—FK to AspNetUsers(Id).
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Navigation to the Employee (User).
        /// </summary>
        public virtual User Employee { get; set; }

        /// <summary>
        /// The leave type—FK to LeaveTypes(Id).
        /// </summary>
        public int LeaveTypeId { get; set; }

        /// <summary>
        /// Navigation to the LeaveType.
        /// </summary>
        public virtual LeaveType LeaveType { get; set; }

        /// <summary>
        /// Calendar year this balance applies to (e.g. 2025).
        /// </summary>
        public short Year { get; set; }

        /// <summary>
        /// Number of days already used this year.
        /// </summary>
        public decimal UsedDays { get; set; }

        /// <summary>
        /// Remaining days (AnnualQuota – UsedDays).
        /// </summary>
        public decimal RemainingDays { get; set; }
    }
}
