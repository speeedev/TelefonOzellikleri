using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TelefonOzellikleri.Cache;
using TelefonOzellikleri.Data;
using TelefonOzellikleri.Helpers;
using TelefonOzellikleri.Models;

namespace TelefonOzellikleri.Controllers
{
    public class PageController : Controller
    {
        private const int PageCacheMinutes = 360; // 6 hours

        private readonly ILogger<PageController> _logger;
        private readonly TelefonOzellikleriDbContext _context;
        private readonly IMemoryCache _cache;

        public PageController(ILogger<PageController> logger, TelefonOzellikleriDbContext context, IMemoryCache cache)
        {
            _logger = logger;
            _context = context;
            _cache = cache;
        }

        [Route("{slug:pageSlug}")]
        public async Task<IActionResult> Index(string slug)
        {
            var page = await _cache.GetOrCreateAsync(CacheKeys.PageDetail(slug), async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(PageCacheMinutes);

                var p = await _context.Pages
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Slug == slug);

                if (p == null)
                {
                    _logger.LogWarning("Page not found: {Slug}", slug);
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1); // Short TTL for newly added pages
                    return (Page?)null;
                }

                _logger.LogInformation("Page loaded from DB and cached: {Slug}", slug);
                return p;
            });

            if (page == null)
                return NotFound();

            var title = SeoHelper.TruncateTitle(page.PageTitle);
            ViewData["Title"] = string.IsNullOrEmpty(title) ? (page.PageTitle ?? "Sayfa") : title;
            ViewData["Description"] = SeoHelper.TruncateDescription(page.PageDescription);

            return View(page);
        }
    }
}
