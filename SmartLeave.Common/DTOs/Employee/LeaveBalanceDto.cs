namespace SmartLeave.Common.DTOs.Employee
{
    public class LeaveBalanceDto
    {
        public string LeaveType { get; set; } = string.Empty;
        public decimal UsedDays { get; set; }
        public decimal RemainingDays { get; set; }
        public decimal Allowance { get; set; }
    }
}
