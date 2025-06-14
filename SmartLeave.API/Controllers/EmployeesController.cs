using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLeave.Common.DTOs.Admin;
using SmartLeave.DAL.Data;

namespace SmartLeave.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DropdownItemDto>>> GetEmployees()
        {
            var employees = await (
                   from user in _context.Users
                   join userRole in _context.UserRoles on user.Id equals userRole.UserId
                   join role in _context.Roles on userRole.RoleId equals role.Id
                   where role.Name == "Employee"
                   select new DropdownItemDto
                   {
                       Id = user.Id,
                       Name = user.UserName
                   }
               ).ToListAsync();

            return Ok(employees);
        }
    }
}
