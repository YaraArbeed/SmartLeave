using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLeave.Common.DTOs.Admin;
using SmartLeave.DAL.Data;

namespace SmartLeave.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DropdownItemDto>>> GetTeams()
        {
            var teams = await _context.Teams
                .Select(t => new DropdownItemDto { Id = t.Id.ToString(), Name = t.Name })
                .ToListAsync();

            return Ok(teams);
        }
    }
}
