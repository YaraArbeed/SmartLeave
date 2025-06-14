using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLeave.Common.DTOs.Admin;
using SmartLeave.DAL.Data;

namespace SmartLeave.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OfficesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DropdownItemDto>>> GetOffices()
        {
            var offices = await _context.Offices
                .Select(o => new DropdownItemDto { Id = o.Id.ToString(), Name = o.Name })
                .ToListAsync();

            return Ok(offices);
        }
    }
}
