using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.DAL.Entities
{
    public class User : IdentityUser
    {
        // 1) RoleId is inherited via IdentityUser.Roles → use UserManager to assign.

        /// <summary>
        /// If the user is an Employee, this points to the Team they belong to.
        /// If the user is a Manager or Admin, this can be null (or point to their own team).
        /// </summary>
        public int? TeamId { get; set; }

        /// <summary>
        /// If this user is an Employee, this points to their Manager's UserId.
        /// For a Manager or Admin, leave it null.
        /// </summary>
        public string? ManagerId { get; set; }

        /// <summary>
        /// e.g. "Software Engineer II" or "Sales Associate"
        /// </summary>
        public string? Position { get; set; }

        /// <summary>
        /// FK to Offices table
        /// </summary>
        public int? OfficeId { get; set; }

        /// <summary>
        /// FK to Countries table
        /// </summary>
        public int? CountryId { get; set; }

        /// <summary>
        /// When they were hired
        /// </summary>
        public DateTime? DateHired { get; set; }

        /// <summary>
        /// Soft-delete flag; defaults to true
        /// </summary>
        public bool IsActive { get; set; } = true;
        // ─── Navigation properties ───────────────────────────────────────────────────────

        /// <summary>
        /// If TeamId is not null, this points to the Team entity.
        /// </summary>
        public virtual Team? Team { get; set; }

        /// <summary>
        /// If ManagerId is not null, this points to the ApplicationUser who is this user’s manager.
        /// </summary>
        public virtual User? Manager { get; set; }

        /// <summary>
        /// If this user is a Manager of a Team, any employees reporting to them appear here:
        /// </summary>
        public virtual ICollection<User> DirectReports { get; set; }

        /// <summary>
        /// If OfficeId is not null, this points to the Office entity.
        /// </summary>
        public  Office? Office { get; set; }

        /// <summary>
        /// If CountryId is not null, this points to the Country entity.
        /// </summary>
        public Country? Country { get; set; }

        /// <summary>
        /// The list of Leave entities this user has applied for.
        /// </summary>
        public virtual ICollection<Leave> LeavesApplied { get; set; }

        /// <summary>
        /// The list of Leave entities this user has approved (if they are a Manager/Admin).
        /// </summary>
        public virtual ICollection<Leave> LeavesApproved { get; set; }

        /// <summary>
        /// Any audit logs created by this user (if they performed an approval or user creation, etc.).
        /// </summary>
        public virtual ICollection<AuditLog> AuditLogsCreated { get; set; }

        /// <summary>
        /// Any company events this user created (e.g. “Company establishment”).
        /// </summary>
        public virtual ICollection<Event> EventsCreated { get; set; }
        /// <summary>
        /// If this user leads one or more teams, those teams go here.
        /// </summary>
        public ICollection<Team> ManagedTeams { get; set; }
        /// <summary>
        /// All LeaveBalance records for this employee (one per LeaveType per year).
        /// </summary>
        public ICollection<LeaveBalance> LeaveBalances { get; set; }

    }
}
