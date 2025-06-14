using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLeave.Common.DTOs.Admin;
using SmartLeave.DAL.Data;

namespace SmartLeave.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CountriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DropdownItemDto>>> GetCountries()
        {
            var countries = await _context.Countries
                .Select(c => new DropdownItemDto { Id = c.Id.ToString(), Name = c.Name })
                .ToListAsync();

            return Ok(countries);
        }
    }
}
