using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLeave.BLL.Interfaces;
using SmartLeave.Common.DTOs.Manager;

namespace SmartLeave.API.Controllers
{
    [Authorize(Roles = "Manager")]
    [Route("api/manager/leaves")]
    [ApiController]
    public class ManagerLeavesController : ControllerBase
    {
        private readonly ILeaveService _service;
        public ManagerLeavesController(ILeaveService service) => _service = service;

        [HttpGet("pending")]
        public async Task<IActionResult> Pending()
        {
            var email = User.Identity!.Name!;
            var list = await _service.GetPendingForManagerAsync(email);
            return Ok(list);
        }

        [HttpPost("decision")]
        public async Task<IActionResult> Decide([FromBody] LeaveDecisionDto dto)
        {
            var email = User.Identity!.Name!;
            var ok = await _service.DecideAsync(email, dto);
            return ok ? Ok("Decision saved") : BadRequest("Not allowed or already decided");
        }
    }
}
