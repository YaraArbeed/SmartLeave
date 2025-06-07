using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SmartLeave.API.DTOs;
using SmartLeave.MVC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmartLeave.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _httpClient.PostAsJsonAsync("api/Accounts/authenticate", new
            {
                Email = model.Email,
                Password = model.Password
            });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Login failed");
                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (result.Is2FactorRequired)
            {
                TempData["email"] = model.Email;
                return RedirectToAction("TwoFactor");
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(result.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.Name, model.Email));

            foreach (var roleClaim in token.Claims.Where(c => c.Type == ClaimTypes.Role))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));
            }


            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Role-based redirection
            if (principal.IsInRole("Admin"))
                return RedirectToAction("Dashboard", "Admin");
            if (principal.IsInRole("Manager"))
                return RedirectToAction("Dashboard", "Manager");

            return RedirectToAction("Dashboard", "Employee");
        }
        // This is already inside your SmartLeave.MVC.Controllers.AccountController
        [HttpGet]
        public IActionResult TwoFactor()
        {
            if (!TempData.ContainsKey("email"))
                return RedirectToAction("Login");

            return View(new TwoFactorViewModel
            {
                Email = TempData["email"]?.ToString()
            });
        }

        [HttpPost]
        public async Task<IActionResult> TwoFactor(TwoFactorViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _httpClient.PostAsJsonAsync("api/Accounts/twofactor", new
            {
                Email = model.Email,
                Token = model.Token,
                Provider = model.Provider
            });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid token");
                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(result.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.Name, model.Email!));

            foreach (var roleClaim in token.Claims)
            {
                Console.WriteLine($"JWT Claim: {roleClaim.Type} = {roleClaim.Value}");

                if (roleClaim.Type == ClaimTypes.Role || roleClaim.Type.EndsWith("/role"))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));
                }
            }

            foreach (var claim in identity.Claims)
            {
                Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            }


            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (principal.IsInRole("Admin"))
                return RedirectToAction("Dashboard", "Admin");
            if (principal.IsInRole("Manager"))
                return RedirectToAction("Dashboard", "Manager");

            return RedirectToAction("Dashboard", "Employee");
        }

    }
}
