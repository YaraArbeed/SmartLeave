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
            var (success, message) = await _leaveService.ApplyForLeaveAsync(email, dto);

            if (!success)
                return BadRequest(new { success = false, message });

            return Ok(new { success = true, message });
        }



        [HttpGet("mine")]
        public async Task<IActionResult> GetMyLeaves([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var email = User.Identity!.Name!;
            var leaves = await _leaveService.GetMyLeavesAsync(email,page,pageSize);
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