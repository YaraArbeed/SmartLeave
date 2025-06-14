using System.Collections.Generic;
using System.Linq;

namespace SmartLeave.MVC.Models
{
    public class AdminDashboardViewModel
    {
        public string AdminName { get; set; } = string.Empty;
        public List<AdminLeaveModel> AllLeaves { get; set; } = new();
        public AdminFilterModel Filter { get; set; } = new();
        public FilterOptionsModel FilterOptions { get; set; } = new();

        public int TotalCount => AllLeaves.Count;
        public int PendingCount => AllLeaves.Count(l => l.Status == "Pending");
        public int ApprovedCount => AllLeaves.Count(l => l.Status == "Approved");
        public int RejectedCount => AllLeaves.Count(l => l.Status == "Rejected");
    }
}
