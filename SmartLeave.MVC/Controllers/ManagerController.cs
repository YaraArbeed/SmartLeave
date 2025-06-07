using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartLeave.MVC.Controllers
{
    public class ManagerController : Controller
    {
        [Authorize(Roles = "Manager")]
        public IActionResult Dashboard()
        {
            return View(); // Create Views/Manager/Dashboard.cshtml
        }
    }
}
