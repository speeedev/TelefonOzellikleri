using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelefonOzellikleri.Data;

namespace TelefonOzellikleri.Controllers
{
    public class PageController : Controller
    {
        private readonly ILogger<PageController> _logger;
        private readonly TelefonOzellikleriDbContext _context;

        public PageController(ILogger<PageController> logger, TelefonOzellikleriDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("{slug:pageSlug}")]
        public async Task<IActionResult> Index(string slug)
        {
            var page = await _context.Pages
                .FirstOrDefaultAsync(p => p.Slug == slug);

            if (page == null)
            {
                _logger.LogWarning("Sayfa bulunamadı: {Slug}", slug);
                return NotFound();
            }

            ViewData["Title"] = page.PageTitle;
            ViewData["Description"] = page.PageDescription;

            _logger.LogInformation("Sayfa görüntülendi: {Slug}", slug);

            return View(page);
        }
    }
}
