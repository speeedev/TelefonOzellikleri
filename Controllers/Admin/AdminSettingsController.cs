using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelefonOzellikleri.Data;
using TelefonOzellikleri.Models;

namespace TelefonOzellikleri.Controllers.Admin
{
    [Authorize]
    public class AdminSettingsController : Controller
    {
        private readonly TelefonOzellikleriDbContext _context;
        private readonly ILogger<AdminSettingsController> _logger;

        public AdminSettingsController(TelefonOzellikleriDbContext context, ILogger<AdminSettingsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Route("derin/settings")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Site Settings";

            var settings = await _context.SiteSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new SiteSetting();
                _context.SiteSettings.Add(settings);
                await _context.SaveChangesAsync();
            }

            return View(settings);
        }

        [Route("derin/settings")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SiteSetting model)
        {
            var settings = await _context.SiteSettings.FirstOrDefaultAsync();
            if (settings == null)
                return NotFound();

            settings.SiteName = model.SiteName;
            settings.SiteTitle = model.SiteTitle;
            settings.SiteDescription = model.SiteDescription;
            settings.LogoUrl = model.LogoUrl;
            settings.FaviconUrl = model.FaviconUrl;
            settings.XUrl = model.XUrl;
            settings.InstagramUrl = model.InstagramUrl;
            settings.YoutubeUrl = model.YoutubeUrl;
            settings.FooterText = model.FooterText;
            settings.IsMaintenanceMode = model.IsMaintenanceMode;
            settings.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Site settings updated.");

            TempData["Success"] = "Settings saved successfully.";
            return RedirectToAction("Index");
        }
    }
}
