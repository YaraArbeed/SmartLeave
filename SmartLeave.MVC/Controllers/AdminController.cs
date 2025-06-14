using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLeave.MVC.Models;
using SmartLeave.MVC.Services;
using System;
using System.Threading.Tasks;

namespace SmartLeave.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IApiService _apiService;

        public AdminController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Dashboard(AdminFilterModel filter)
        {
            try
            {
                var viewModel = new AdminDashboardViewModel
                {
                    AdminName = User.Identity?.Name ?? "Admin",
                    Filter = filter ?? new AdminFilterModel()
                };

                // Get filter options for dropdowns
                viewModel.FilterOptions = await _apiService.GetAdminFilterOptionsAsync();

                // Get all leave requests using the filter
                viewModel.AllLeaves = await _apiService.GetAdminLeavesAsync(filter);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading admin dashboard: {ex.Message}");
                ViewBag.ErrorMessage = "Unable to load admin dashboard. Please try again.";
                return View(new AdminDashboardViewModel
                {
                    AdminName = User.Identity?.Name ?? "Admin",
                    Filter = filter ?? new AdminFilterModel()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessLeaveDecision(LeaveDecisionModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please provide a valid decision and note.";
                return RedirectToAction("Dashboard");
            }

            try
            {
                Console.WriteLine($"Processing leave decision - LeaveId: {model.LeaveId}, IsApproved: {model.IsApproved}");
                var success = await _apiService.ProcessAdminLeaveDecisionAsync(model);

                if (success)
                {
                    TempData["SuccessMessage"] = $"Leave request has been {(model.IsApproved ? "approved" : "rejected")} successfully. Employee will be notified via email.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to process leave decision. Please try again.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing leave decision: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while processing the decision. Please try again.";
            }

            return RedirectToAction("Dashboard");
        }
    }
}
