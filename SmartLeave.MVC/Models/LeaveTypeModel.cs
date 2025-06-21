namespace SmartLeave.MVC.Models
{
    public class LeaveTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal AnnualQuota { get; set; }
        public string? Description { get; set; } // Optional for display
        public string Color { get; set; } = "#007bff"; // You can assign by ID if needed
    }

    public class EmployeeDashboardViewModel
    {
        public List<LeaveResponseModel> AllLeaves { get; set; } = new(); // NEW
        public List<LeaveResponseModel> RecentLeaves => AllLeaves
            .OrderByDescending(l => l.StartDate)
            .Take(5)
            .ToList();

        public List<LeaveBalanceModel> LeaveBalances { get; set; } = new();
        public List<PublicHolidayModel> UpcomingHolidays { get; set; } = new();
        public List<LeaveTypeModel> LeaveTypes { get; set; } = new();
        public string EmployeeName { get; set; } = string.Empty;
        public decimal TotalRemainingDays { get; set; }
        public string ActiveSection { get; set; } = "dashboard";

    }

}