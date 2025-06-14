using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLeave.BLL.Interfaces;
using SmartLeave.Common.DTOs.Admin;
using SmartLeave.Common.DTOs.Manager;

namespace SmartLeave.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/reports")]
    [ApiController]
    public class AdminReportsController : ControllerBase
    {
        private readonly ILeaveService _svc;
        public AdminReportsController(ILeaveService svc) => _svc = svc;

        // 1. Get filters → report
        [HttpGet("leaves")]
        public async Task<IActionResult> Leaves([FromQuery] LeaveReportFilterDto f)
            => Ok(await _svc.GetReportAsync(f));

        // 2. decision
        [HttpPost("decision")]
        public async Task<IActionResult> Decide([FromBody] LeaveDecisionDto dto)
        {
            var ok = await _svc.AdminDecideAsync(User.Identity!.Name!, dto);
            return ok ? Ok("Saved") : BadRequest("Invalid or already decided");
        }
    }
}
