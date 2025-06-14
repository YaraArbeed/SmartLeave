using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLeave.Common.DTOs.Admin;

namespace SmartLeave.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStatuses()
        {
            var statuses = new[]
            {
            new DropdownItemDto { Id = "Pending", Name = "Pending" },
            new DropdownItemDto { Id = "Approved", Name = "Approved" },
            new DropdownItemDto { Id = "Rejected", Name = "Rejected" }
        };

            return Ok(statuses);
        }
    }
}
