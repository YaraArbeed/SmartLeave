using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLeave.BLL.Interfaces;
using SmartLeave.Common.DTOs.Employee;
namespace SmartLeave.API.Controllers
{
    [Authorize(Roles = "Employee")]
    [Route("api/[controller]")]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        private readonly ILeaveService _leaveService;

        public LeavesController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestLeave([FromForm] LeaveRequestDto dto)
        {
            var email = User.Identity!.Name!;
            var result = await _leaveService.ApplyForLeaveAsync(email, dto);
            return result ? Ok("Leave submitted successfully") : BadRequest("Not enough balance or overlapping dates");
        }

        [HttpGet("mine")]
        public async Task<IActionResult> GetMyLeaves()
        {
            var email = User.Identity!.Name!;
            var leaves = await _leaveService.GetMyLeavesAsync(email);
            return Ok(leaves);
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetMyBalance()
        {
            var email = User.Identity!.Name!;
            var balance = await _leaveService.GetMyLeaveBalancesAsync(email);
            return Ok(balance);
        }
    }
}