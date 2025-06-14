using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace SmartLeave.MVC.Models
{
    public class FilterOptionsModel
    {
        public List<SelectListItem> Employees { get; set; } = new();
        public List<SelectListItem> Teams { get; set; } = new();
        public List<SelectListItem> LeaveTypes { get; set; } = new();
        public List<SelectListItem> Offices { get; set; } = new();
        public List<SelectListItem> Countries { get; set; } = new();
        public List<SelectListItem> Approvers { get; set; } = new();
        public List<SelectListItem> Statuses { get; set; } = new();
    }
}
