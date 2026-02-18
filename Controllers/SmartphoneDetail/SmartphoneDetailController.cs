using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelefonOzellikleri.Data;
using TelefonOzellikleri.Models;
using TelefonOzellikleri.Models.ViewModels;

namespace TelefonOzellikleri.Controllers
{
    public class SmartphoneDetailController : Controller
    {
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

    string cacheKey = $"phone_detail_{slug.ToLower()}";

    if (!_cache.TryGetValue(cacheKey, out SmartphoneDetailViewModel viewModel))
    {
        var phone = await _context.Smartphones
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Slug == slug);

        if (phone == null)
        {
            _logger.LogWarning("Telefon bulunamadı: {Slug}", slug);
            return NotFound();
        }

        var brand = await _context.Brands
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == phone.BrandId);

        if (brand == null)
        {
            _logger.LogWarning("Marka bulunamadı: BrandId={BrandId}", phone.BrandId);
            return NotFound();
        }

        Series? series = null;
        if (phone.SeriesId.HasValue)
        {
            series = await _context.Series
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == phone.SeriesId.Value);
        }

        viewModel = new SmartphoneDetailViewModel
        {
            Phone = phone,
            Brand = brand,
            Series = series
        };

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(6));

        _cache.Set(cacheKey, viewModel, cacheOptions);

        _logger.LogInformation("Telefon DB'den çekildi ve cache'e alındı: {Slug}", slug);
    }
    else
    {
        _logger.LogInformation("Telefon cache'den getirildi: {Slug}", slug);
    }

    ViewData["Title"] = $"{viewModel.Brand.Name} {viewModel.Phone.ModelName} Özellikleri";
    ViewData["Description"] =
        $"{viewModel.Brand.Name} {viewModel.Phone.ModelName} teknik özellikleri, kamera, ekran, batarya, işlemci ve daha fazlası.";

    return View(viewModel);
}

    }
}
