using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.Common.DTOs.Manager
{
    public class LeaveDecisionDto
    {
        public int LeaveId { get; set; }
        public bool IsApproved { get; set; }     // true = approve, false = reject
        public string? Note { get; set; }     // manager’s note
    }
}
