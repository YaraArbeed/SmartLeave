using Microsoft.AspNetCore.Mvc;
using SmartLeave.MVC.Models;
using System.Text;
using System.Text.Json;

namespace SmartLeave.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:5001/api/accounts/register", content);

            if (response.IsSuccessStatusCode)
            {
                // Registration success — go to login page or notify user
                TempData["Message"] = "Registered successfully! Please log in.";
                return RedirectToAction("Login");
            }
            else
            {
                // Read errors from API
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<RegistrationResponseDto>(responseBody,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(model);
            }
        }

        public class RegistrationResponseDto
        {
            public bool IsSuccessfulRegistration { get; set; }
            public IEnumerable<string> Errors { get; set; }
        }
    }
}

