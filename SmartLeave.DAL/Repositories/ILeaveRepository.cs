using SmartLeave.Common.DTOs.Admin;
using SmartLeave.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.DAL.Repositories
{
    public interface ILeaveRepository
    {
        //--------------------------------Employee---------------------------------------
        Task<bool> AddLeaveAsync(Leave leave);
        Task<bool> HasOverlappingLeave(string employeeId, DateTime start, DateTime end);
        Task<List<Leave>> GetLeavesByEmployeeIdAsync(string employeeId);
        Task<List<LeaveBalance>> GetLeaveBalancesAsync(string employeeId);
        Task<LeaveBalance?> GetLeaveBalanceAsync(string employeeId, int leaveTypeId);
        //---------------------------------Manager----------------------------------------
        Task<Leave?> GetByIdAsync(int leaveId);
        Task<List<Leave>> GetPendingByTeamAsync(int teamId);
        Task RestoreBalanceAsync(string employeeId, int leaveTypeId, decimal days);
        Task<bool> UpdateAsync(Leave leave);
        //-----------------------------------Admin-------------------------------------------
        Task<List<Leave>> QueryLeavesAsync(LeaveReportFilterDto f);
    }
}
