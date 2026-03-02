using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TelefonOzellikleri.Cache;
using TelefonOzellikleri.Data;
using TelefonOzellikleri.Helpers;
using TelefonOzellikleri.Models;
using TelefonOzellikleri.Models.ViewModels;

namespace TelefonOzellikleri.Controllers
{
    public class SmartphoneDetailController : Controller
    {
        private const int PhoneCacheMinutes = 360; // 6 hours

        private readonly ILogger<SmartphoneDetailController> _logger;
        private readonly TelefonOzellikleriDbContext _context;
        private readonly IMemoryCache _cache;

        public SmartphoneDetailController(ILogger<SmartphoneDetailController> logger, TelefonOzellikleriDbContext context, IMemoryCache cache)
        {
            _logger = logger;
            _context = context;
            _cache = cache;
        }

        [Route("{slug:smartphoneSlug}")]
        [ResponseCache(NoStore = true, Duration = 0)]
        public async Task<IActionResult> Index(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return NotFound();

            var viewModel = await _cache.GetOrCreateAsync(CacheKeys.PhoneDetail(slug), async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(PhoneCacheMinutes);
                entry.Size = 1;

                var phone = await _context.Smartphones
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Slug == slug);

                if (phone == null)
                {
                    _logger.LogWarning("Phone not found: {Slug}", slug);
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1); // Short TTL for newly added phones
                    return (SmartphoneDetailViewModel?)null;
                }

                var brand = await _context.Brands
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Id == phone.BrandId);

                if (brand == null)
                {
                    _logger.LogWarning("Brand not found: BrandId={BrandId}", phone.BrandId);
                    return (SmartphoneDetailViewModel?)null;
                }

                Series? series = null;
                if (phone.SeriesId.HasValue)
                {
                    series = await _context.Series
                        .AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == phone.SeriesId.Value);
                }

                var sameBrandPhones = await _context.Smartphones
                    .AsNoTracking()
                    .Where(s => s.BrandId == phone.BrandId && s.Id != phone.Id)
                    .OrderByDescending(s => s.ReleaseDate ?? DateOnly.MinValue)
                    .ThenByDescending(s => s.Id)
                    .Take(6)
                    .Join(_context.Brands,
                        p => p.BrandId,
                        b => b.Id,
                        (p, b) => new LatestPhoneItem
                        {
                            Slug = p.Slug,
                            ModelName = p.ModelName,
                            BrandName = b.Name,
                            BrandLogoUrl = b.LogoUrl,
                            MainImageUrl = p.MainImageUrl,
                            Chipset = p.Chipset,
                            BatteryCapacity = p.BatteryCapacity,
                            Cam1Res = p.Cam1Res,
                            ScreenSize = p.ScreenSize,
                            ReleaseDate = p.ReleaseDate
                        })
                    .ToListAsync();

                return new SmartphoneDetailViewModel
                {
                    Phone = phone,
                    Brand = brand,
                    Series = series,
                    SameBrandPhones = sameBrandPhones
                };
            });

            if (viewModel == null)
                return NotFound();

            ViewData["Title"] = SeoHelper.TruncateTitle($"{viewModel.Brand.Name} {viewModel.Phone.ModelName} Özellikleri");
            ViewData["Description"] = SeoHelper.TruncateDescription(
                $"{viewModel.Brand.Name} {viewModel.Phone.ModelName} teknik özellikleri, kamera, ekran, batarya, işlemci ve diğer detayları.");
            ViewData["OgTitle"] = $"{viewModel.Brand.Name} {viewModel.Phone.ModelName} Özellikleri";
            ViewData["OgDescription"] = ViewData["Description"];
            if (!string.IsNullOrEmpty(viewModel.Phone.MainImageUrl))
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                ViewData["OgImage"] = viewModel.Phone.MainImageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                    ? viewModel.Phone.MainImageUrl
                    : baseUrl + (viewModel.Phone.MainImageUrl.StartsWith("/") ? "" : "/") + viewModel.Phone.MainImageUrl;
            }
            ViewData["CanonicalUrl"] = $"{Request.Scheme}://{Request.Host}/{slug}";

            return View(viewModel);
        }

    }
}
