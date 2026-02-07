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

        public SmartphoneDetailController(ILogger<SmartphoneDetailController> logger, TelefonOzellikleriDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("{slug:smartphoneSlug}")]
        public async Task<IActionResult> Index(string slug)
        {
            var phone = await _context.Smartphones
                .FirstOrDefaultAsync(p => p.Slug == slug);

            if (phone == null)
            {
                _logger.LogWarning("Telefon bulunamadı: {Slug}", slug);
                return NotFound();
            }

            var brand = await _context.Brands
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
                    .FirstOrDefaultAsync(s => s.Id == phone.SeriesId.Value);
            }

            var viewModel = new SmartphoneDetailViewModel
            {
                Phone = phone,
                Brand = brand,
                Series = series
            };

            ViewData["Title"] = $"{brand.Name} {phone.ModelName} Özellikleri";
            ViewData["Description"] = $"{brand.Name} {phone.ModelName} teknik özellikleri, kamera, ekran, batarya, işlemci ve daha fazlası.";

            _logger.LogInformation("Telefon detay görüntülendi: {Slug}", slug);

            return View(viewModel);
        }
    }
}
