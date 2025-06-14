using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SmartLeave.Common.DTOs.Admin
{
    public class LeaveReportFilterDto
    {
        public DateTime? FromDate { get; set; }  // filter start of range
        public DateTime? ToDate { get; set; }  // filter end of range
        public int? TeamId { get; set; }  // filter by team
        public int? LeaveTypeId { get; set; }  // vacation, sick, etc.
        public int? OfficeId { get; set; }  // which office
        public int? CountryId { get; set; }  // which country
        public string? Status { get; set; }  // Pending/Approved/Rejected
        public string? ApproverId { get; set; }  // user ID of approver
        public string? EmployeeId { get; set; }  // specific employee
    }
}
