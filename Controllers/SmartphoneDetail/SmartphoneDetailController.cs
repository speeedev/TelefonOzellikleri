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
        public async Task<IActionResult> Index(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return NotFound();

            var viewModel = await _cache.GetOrCreateAsync(CacheKeys.PhoneDetail(slug), async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(PhoneCacheMinutes);

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

                _logger.LogInformation("Phone loaded from DB and cached: {Slug}", slug);
                return new SmartphoneDetailViewModel
                {
                    Phone = phone,
                    Brand = brand,
                    Series = series
                };
            });

            if (viewModel == null)
                return NotFound();

            ViewData["Title"] = SeoHelper.TruncateTitle($"{viewModel.Brand.Name} {viewModel.Phone.ModelName} Özellikleri");
            ViewData["Description"] = SeoHelper.TruncateDescription(
                $"{viewModel.Brand.Name} {viewModel.Phone.ModelName} teknik özellikleri, kamera, ekran, batarya, işlemci ve diğer detayları.");

            return View(viewModel);
        }

    }
}
