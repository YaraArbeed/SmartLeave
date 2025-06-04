using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.DAL.Entities
{
    public class LeaveType
    {
        /// <summary>
        /// Primary key for LeaveType (int).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// e.g. "Vacation", "Sick", "Unpaid", etc.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Optional description of this leave type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// How many days per year are allocated (e.g. 14.00 for 14 days).
        /// </summary>
        public decimal AnnualQuota { get; set; }

        /// <summary>
        /// All Leave records of this type.
        /// </summary>
        public virtual ICollection<Leave> Leaves { get; set; }

        /// <summary>
        /// All LeaveBalance records that reference this type.
        /// </summary>
        public virtual ICollection<LeaveBalance> LeaveBalances { get; set; }
    }
}
