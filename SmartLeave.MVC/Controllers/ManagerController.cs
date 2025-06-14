using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLeave.MVC.Models;
using SmartLeave.MVC.Services;

namespace SmartLeave.MVC.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly IApiService _apiService;

        public ManagerController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var viewModel = new ManagerDashboardViewModel
                {
                    ManagerName = User.Identity?.Name ?? "Manager"
                };

                // Get pending leave requests
                viewModel.PendingLeaves = await _apiService.GetPendingLeavesAsync();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Unable to load manager dashboard. Please try again.";
                return View(new ManagerDashboardViewModel
                {
                    ManagerName = User.Identity?.Name ?? "Manager"
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
                var success = await _apiService.ProcessLeaveDecisionAsync(model);

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
                TempData["ErrorMessage"] = "An error occurred while processing the decision. Please try again.";
            }

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> GetLeaveDetails(int leaveId)
        {
            try
            {
                var leaveDetails = await _apiService.GetLeaveDetailsAsync(leaveId);
                return Json(leaveDetails);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Failed to load leave details" });
            }
        }
    }
}
