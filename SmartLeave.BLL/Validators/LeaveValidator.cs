using SmartLeave.Common.DTOs.Employee;
using SmartLeave.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.Common.Validators
{
    public class LeaveValidator
    {
        private readonly ILeaveRepository _leaveRepo;

        public LeaveValidator(ILeaveRepository leaveRepo)
        {
            _leaveRepo = leaveRepo;
        }

        public async Task<(bool IsValid, string? ErrorMessage)> ValidateAsync(string employeeId, LeaveRequestDto dto)
        {
            if (dto.StartDate > dto.EndDate)
                return (false, "Start date must be before or equal to end date.");

            var overlap = await _leaveRepo.HasOverlappingLeave(employeeId, dto.StartDate, dto.EndDate);
            if (overlap)
                return (false, "You already have a leave that overlaps with this date range.");

            var days = (decimal)(dto.EndDate - dto.StartDate).TotalDays + 1;
            var balance = await _leaveRepo.GetLeaveBalanceAsync(employeeId, dto.LeaveTypeId);

            if (balance.RemainingDays < days)
                return (false, "Insufficient leave balance.");

            return (true, null);
        }
    }
}
