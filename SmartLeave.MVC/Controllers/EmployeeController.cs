using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartLeave.MVC.Controllers
{
    public class EmployeeController : Controller
    {
        [Authorize(Roles = "Employee")]

        public IActionResult Dashboard()
        {
            return View(); // Create Views/Employee/Dashboard.cshtml
        }
    }
}
