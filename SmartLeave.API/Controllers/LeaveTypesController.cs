using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLeave.Common.DTOs.All;
using SmartLeave.DAL.Data;

namespace SmartLeave.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LeaveTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LeaveTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveTypeDto>>> GetLeaveTypes()
        {
            var leaveTypes = await _context.LeaveTypes
                .Select(x => new LeaveTypeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    AnnualQuota = x.AnnualQuota,
                    Description = x.Description
                })
                .ToListAsync();

            return Ok(leaveTypes);
        }
    }
}

