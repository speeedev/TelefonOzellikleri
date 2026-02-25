using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TelefonOzellikleri.Cache;
using TelefonOzellikleri.Data;
using TelefonOzellikleri.Helpers;
using TelefonOzellikleri.Models.ViewModels;
using TelefonOzellikleri.Services;

namespace TelefonOzellikleri.Controllers
{
    public class HomeController : Controller
    {
        private const int HomePageCacheMinutes = 60;

        private readonly ILogger<HomeController> _logger;
        private readonly TelefonOzellikleriDbContext _context;
        private readonly FeedService _feedService;
        private readonly IMemoryCache _cache;

        public HomeController(ILogger<HomeController> logger, TelefonOzellikleriDbContext context, FeedService feedService, IMemoryCache cache)
        {
            _logger = logger;
            _context = context;
            _feedService = feedService;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _cache.GetOrCreateAsync(CacheKeys.HomePage, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(HomePageCacheMinutes);

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

                var news = await _feedService.GetNewsAsync();

                return new HomeViewModel
                {
                    SiteTitle = siteSettings?.SiteTitle ?? "",
                    SiteDescription = siteSettings?.SiteDescription ?? "",
                    FooterText = siteSettings?.FooterText,
                    News = news,
                    LatestPhones = latestPhones
                };
            });

            if (viewModel == null)
                return NotFound();

            var title = SeoHelper.TruncateTitle(viewModel.SiteTitle);
            var description = SeoHelper.TruncateDescription(viewModel.SiteDescription);
            ViewData["Title"] = string.IsNullOrEmpty(title) ? "Akıllı Telefon Özellikleri ve Karşılaştırmaları" : title;
            ViewData["Description"] = string.IsNullOrEmpty(description) ? "Akıllı telefon teknik özellikleri, kamera, ekran, batarya karşılaştırmaları. En güncel modelleri inceleyin." : description;
            ViewData["FooterText"] = viewModel.FooterText;

            return View(viewModel);
        }
    }
}

