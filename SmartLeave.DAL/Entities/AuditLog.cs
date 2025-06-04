using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.DAL.Entities
{
    public class AuditLog
    {
        /// <summary>
        /// Primary key for AuditLog (int).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The user who performed the action—FK to AspNetUsers(Id).
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Navigation to the User who performed the action.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// e.g. "LeaveApproved", "UserCreated", etc.
        /// </summary>
        public string ActionType { get; set; }

        /// <summary>
        /// The name of the table that was affected—e.g. "Leaves", "Users".
        /// </summary>
        public string TargetTable { get; set; }

        /// <summary>
        /// The primary-key value of the affected row in TargetTable.
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// When the action occurred.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Optional notes or details about the change.
        /// </summary>
        public string Notes { get; set; }
    }
}
