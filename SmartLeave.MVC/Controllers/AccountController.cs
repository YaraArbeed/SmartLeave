using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SmartLeave.MVC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartLeave.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IHttpClientFactory httpClientFactory, ILogger<AccountController> logger)
        {
            _httpClient = httpClientFactory.CreateClient("API");
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                _logger.LogInformation("Attempting login for user: {Email}", model.Email);
                _logger.LogInformation("API Base Address: {BaseAddress}", _httpClient.BaseAddress);

                var loginRequest = new
                {
                    Email = model.Email,
                    Password = model.Password
                };

                _logger.LogInformation("Sending login request to: {Url}", "api/Accounts/authenticate");

                var response = await _httpClient.PostAsJsonAsync("api/Accounts/authenticate", loginRequest);

                _logger.LogInformation("Response Status: {StatusCode}", response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Response Content: {Content}", responseContent);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Login failed for user: {Email}. Status: {StatusCode}",
                        model.Email, response.StatusCode);

                    // Show the actual error from API
                    var errorMessage = "Login failed";
                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        try
                        {
                            var errorResponse = System.Text.Json.JsonSerializer.Deserialize<dynamic>(responseContent);
                            errorMessage = errorResponse?.ToString() ?? "Invalid credentials";
                        }
                        catch
                        {
                            errorMessage = responseContent.Length > 200 ? responseContent.Substring(0, 200) : responseContent;
                        }
                    }

                    ModelState.AddModelError("", $"Login failed: {errorMessage}");
                    return View(model);
                }

                var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

                if (result == null)
                {
                    _logger.LogError("Failed to deserialize response");
                    ModelState.AddModelError("", "Invalid response from server");
                    return View(model);
                }

                _logger.LogInformation("Login successful. 2FA Required: {Is2FA}", result.Is2FactorRequired);

                if (result.Is2FactorRequired)
                {
                    TempData["email"] = model.Email;
                    TempData["SuccessMessage"] = "Please check your email for the verification code.";
                    return RedirectToAction("TwoFactor");
                }

                // Process JWT token
                if (string.IsNullOrEmpty(result.Token))
                {
                    _logger.LogError("No token received from API");
                    ModelState.AddModelError("", "No authentication token received");
                    return View(model);
                }

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(result.Token);

                _logger.LogInformation("JWT Token parsed successfully. Claims count: {ClaimsCount}", token.Claims.Count());

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.Name, model.Email));

                // Log all JWT claims for debugging
                foreach (var claim in token.Claims)
                {
                    _logger.LogInformation("JWT Claim: {Type} = {Value}", claim.Type, claim.Value);

                    if (claim.Type == ClaimTypes.Role || claim.Type.Contains("role"))
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, claim.Value));
                        _logger.LogInformation("Added role claim: {Role}", claim.Value);
                    }
                }

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                HttpContext.Session.SetString("JWTToken", result.Token);

                _logger.LogInformation("User signed in successfully");

                // Role-based redirection
                if (principal.IsInRole("Admin"))
                {
                    _logger.LogInformation("Redirecting to Admin dashboard");
                    return RedirectToAction("Dashboard", "Admin");
                }
                if (principal.IsInRole("Manager"))
                {
                    _logger.LogInformation("Redirecting to Manager dashboard");
                    return RedirectToAction("Dashboard", "Manager");
                }

                _logger.LogInformation("Redirecting to Employee dashboard");
                return RedirectToAction("Dashboard", "Employee");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed during login for user: {Email}", model.Email);
                ModelState.AddModelError("", $"Unable to connect to the server: {ex.Message}");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for user: {Email}", model.Email);
                ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult TwoFactor()
        {
            if (!TempData.ContainsKey("email"))
                return RedirectToAction("Login");

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(new TwoFactorViewModel
            {
                Email = TempData["email"]?.ToString() ?? string.Empty
            });
        }

        [HttpPost]
        public async Task<IActionResult> TwoFactor(TwoFactorViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                _logger.LogInformation("Attempting 2FA for user: {Email}", model.Email);

                var response = await _httpClient.PostAsJsonAsync("api/Accounts/twofactor", new
                {
                    Email = model.Email,
                    Token = model.Token,
                    Provider = model.Provider
                });

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("2FA Response Status: {StatusCode}, Content: {Content}",
                    response.StatusCode, responseContent);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Invalid verification code");
                    return View(model);
                }

                var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

                if (result == null || string.IsNullOrEmpty(result.Token))
                {
                    ModelState.AddModelError("", "Invalid response from server");
                    return View(model);
                }

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(result.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.Name, model.Email));

                foreach (var roleClaim in token.Claims)
                {
                    _logger.LogInformation("2FA JWT Claim: {Type} = {Value}", roleClaim.Type, roleClaim.Value);

                    if (roleClaim.Type == ClaimTypes.Role || roleClaim.Type.Contains("role"))
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));
                    }
                }

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                HttpContext.Session.SetString("JWTToken", result.Token);

                if (principal.IsInRole("Admin"))
                    return RedirectToAction("Dashboard", "Admin");
                if (principal.IsInRole("Manager"))
                    return RedirectToAction("Dashboard", "Manager");

                return RedirectToAction("Dashboard", "Employee");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during two-factor authentication for user: {Email}", model.Email);
                ModelState.AddModelError("", "An error occurred during verification. Please try again.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // Add this action for testing API connectivity
        [HttpGet]
        public async Task<IActionResult> TestApi()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/PublicHolidays/saudi");
                var content = await response.Content.ReadAsStringAsync();

                return Json(new
                {
                    Success = response.IsSuccessStatusCode,
                    StatusCode = response.StatusCode,
                    Content = content,
                    BaseAddress = _httpClient.BaseAddress?.ToString()
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Error = ex.Message,
                    BaseAddress = _httpClient.BaseAddress?.ToString()
                });
            }
        }
    }
}
