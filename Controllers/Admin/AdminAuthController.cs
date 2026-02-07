using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TelefonOzellikleri.Models.ViewModels;

namespace TelefonOzellikleri.Controllers.Admin
{
    public class AdminAuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdminAuthController> _logger;

        public AdminAuthController(IConfiguration configuration, ILogger<AdminAuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [Route("derin/login")]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "AdminDashboard");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [Route("derin/login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var adminUsername = _configuration["AdminSettings:Username"];
            var adminPasswordHash = _configuration["AdminSettings:PasswordHash"];

            if (model.Username != adminUsername || !VerifyPassword(model.Password, adminPasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                _logger.LogWarning("Failed login attempt: {Username}", model.Username);
                return View(model);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, model.Username),
                new(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = model.RememberMe
                    ? DateTimeOffset.UtcNow.AddDays(7)
                    : DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            _logger.LogInformation("Admin logged in: {Username}", model.Username);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "AdminDashboard");
        }

        [Route("derin/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("Admin logged out.");
            return RedirectToAction("Login");
        }

        private static bool VerifyPassword(string password, string? storedHash)
        {
            if (string.IsNullOrEmpty(storedHash))
                return false;

            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
