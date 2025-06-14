using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLeave.BLL.Interfaces;

namespace SmartLeave.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicHolidaysController : ControllerBase
    {
        private readonly IPublicHolidayService _service;

        public PublicHolidaysController(IPublicHolidayService service)
        {
            _service = service;
        }

        [HttpGet("{country}")]
        public async Task<IActionResult> Get(string country)
        {
            var data = await _service.GetPublicHolidaysAsync(country);
            return Ok(data);
        }
    }
}
