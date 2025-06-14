using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLeave.MVC.Models;
using SmartLeave.MVC.Services;

namespace SmartLeave.MVC.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly IApiService _apiService;

        public EmployeeController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Dashboard(string section = "dashboard")
        {
            try
            {
                var viewModel = new EmployeeDashboardViewModel
                {
                    EmployeeName = User.Identity?.Name ?? "Employee",
                    ActiveSection = section
                };

                viewModel.LeaveBalances = await _apiService.GetMyLeaveBalancesAsync();
                viewModel.TotalRemainingDays = viewModel.LeaveBalances.Sum(b => b.RemainingDays);

                // Get all leaves once
                viewModel.AllLeaves = await _apiService.GetMyLeavesAsync();

                // Get upcoming holidays
                viewModel.UpcomingHolidays = await _apiService.GetPublicHolidaysAsync("saudi");
                viewModel.UpcomingHolidays = viewModel.UpcomingHolidays
                    .Where(h => h.HolidayDate >= DateTime.Now)
                    .OrderBy(h => h.HolidayDate)
                    .Take(5)
                    .ToList();

                // Get leave types
                viewModel.LeaveTypes = await _apiService.GetLeaveTypesAsync();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Unable to load dashboard data. Please try again.";
                return View(new EmployeeDashboardViewModel
                {
                    EmployeeName = User.Identity?.Name ?? "Employee",
                    ActiveSection = section
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RequestLeave(LeaveRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields.";
                return RedirectToAction("Dashboard");
            }

            if (model.StartDate > model.EndDate)
            {
                TempData["ErrorMessage"] = "End date must be after start date.";
                return RedirectToAction("Dashboard");
            }

            try
            {
                var success = await _apiService.RequestLeaveAsync(model);

                if (success)
                {
                    TempData["SuccessMessage"] = "Leave request submitted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to submit leave request. Please check your leave balance and dates.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while submitting your request. Please try again.";
            }

            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> MyLeaves()
        {
            var viewModel = new EmployeeDashboardViewModel
            {
                EmployeeName = User.Identity?.Name ?? "Employee",
                ActiveSection = "myleaves"
            };

            viewModel.AllLeaves = await _apiService.GetMyLeavesAsync();

            return View("MyLeaves", viewModel); // Uses new view
        }


    }
}
