using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelefonOzellikleri.Data;
using TelefonOzellikleri.Models.ViewModels;

namespace TelefonOzellikleri.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TelefonOzellikleriDbContext _context;
        public HomeController(ILogger<HomeController> logger, TelefonOzellikleriDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var siteSettings = await _context.SiteSettings.FirstOrDefaultAsync();

            var latestPhones = await _context.Smartphones
                .OrderByDescending(s => s.Id)
                .Take(12)
                .Join(_context.Brands,
                    phone => phone.BrandId,
                    brand => brand.Id,
                    (phone, brand) => new LatestPhoneItem
                    {
                        Slug = phone.Slug,
                        ModelName = phone.ModelName,
                        BrandName = brand.Name,
                        BrandLogoUrl = brand.LogoUrl,
                        MainImageUrl = phone.MainImageUrl,
                        Chipset = phone.Chipset,
                        BatteryCapacity = phone.BatteryCapacity,
                        Cam1Res = phone.Cam1Res,
                        ScreenSize = phone.ScreenSize,
                        ReleaseDate = phone.ReleaseDate
                    })
                .ToListAsync();

            var viewModel = new HomeViewModel
            {
                SiteTitle = siteSettings?.SiteTitle ?? "",
                SiteDescription = siteSettings?.SiteDescription ?? "",
                LatestPhones = latestPhones
            };

            ViewData["Title"] = viewModel.SiteTitle;
            ViewData["Description"] = viewModel.SiteDescription;
            ViewData["FooterText"] = siteSettings?.FooterText;

            return View(viewModel);
        }
    }
}

