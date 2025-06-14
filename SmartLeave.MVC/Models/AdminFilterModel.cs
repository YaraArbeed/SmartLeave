using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLeave.MVC.Models
{
    public class AdminFilterModel
    {
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Employee")]
        public string? EmployeeId { get; set; }

        [Display(Name = "Team")]
        public int? TeamId { get; set; }

        [Display(Name = "Status")]
        public string? Status { get; set; }

        [Display(Name = "Leave Type")]
        public int? LeaveTypeId { get; set; }

        [Display(Name = "Office")]
        public int? OfficeId { get; set; }

        [Display(Name = "Country")]
        public int? CountryId { get; set; }

        [Display(Name = "Approver")]
        public string? ApproverId { get; set; }
    }
}
