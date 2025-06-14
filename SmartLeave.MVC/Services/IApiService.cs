using SmartLeave.MVC.Models;

namespace SmartLeave.MVC.Services
{
    public interface IApiService
    {
        Task<List<LeaveResponseModel>> GetMyLeavesAsync();
        Task<List<LeaveBalanceModel>> GetMyLeaveBalancesAsync();
        Task<bool> RequestLeaveAsync(LeaveRequestModel request);
        Task<List<PublicHolidayModel>> GetPublicHolidaysAsync(string country);
        Task<List<LeaveTypeModel>> GetLeaveTypesAsync();
        //manager 
        Task<List<PendingLeaveModel>> GetPendingLeavesAsync();
        Task<bool> ProcessLeaveDecisionAsync(LeaveDecisionModel decision);
        Task<PendingLeaveModel> GetLeaveDetailsAsync(int leaveId);
        //Admin
        Task<List<AdminLeaveModel>> GetAdminLeavesAsync(AdminFilterModel filter);
        Task<FilterOptionsModel> GetAdminFilterOptionsAsync();
        Task<bool> ProcessAdminLeaveDecisionAsync(LeaveDecisionModel decision);

    }
}
