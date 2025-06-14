using SmartLeave.Common.DTOs.Admin;
using SmartLeave.Common.DTOs.Employee;
using SmartLeave.Common.DTOs.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SmartLeave.BLL.Interfaces
{
    public interface ILeaveService
    {
        //-------------------------------Employee---------------------------------
        Task<(bool Success, string Message)> ApplyForLeaveAsync(string userEmail, LeaveRequestDto dto);
        Task<List<LeaveResponseDto>> GetMyLeavesAsync(string userEmail, int page = 1, int pageSize = 10);
        Task<List<LeaveBalanceDto>> GetMyLeaveBalancesAsync(string userEmail);
        //----------------------------------Manager--------------------------------
        Task<List<LeaveResponseDto>> GetPendingForManagerAsync(string managerEmail, int page = 1, int pageSize = 10);
        Task<bool> DecideAsync(string managerEmail, LeaveDecisionDto dto);
        //---------------------------------Admin------------------------------------
        Task<List<LeaveResponseDto>> GetReportAsync(LeaveReportFilterDto filter);
        Task<bool> AdminDecideAsync(string adminEmail, LeaveDecisionDto decision);
    }
}
