using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelefonOzellikleri.Data;

namespace TelefonOzellikleri.Controllers
{
    public class SitemapController : Controller
    {
        private readonly TelefonOzellikleriDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SitemapController(TelefonOzellikleriDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [Route("sitemap.xml")]
        [ResponseCache(Duration = 3600)]
        public async Task<IActionResult> Index()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = $"{request?.Scheme}://{request?.Host}";

            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

            var urlElements = new List<XElement>();

            // Home page
            urlElements.Add(CreateUrlElement(ns, baseUrl, "/", "daily", "1.0"));

            // Phones
            var phones = await _context.Smartphones
                .Select(s => s.Slug)
                .ToListAsync();

            foreach (var slug in phones)
            {
                urlElements.Add(CreateUrlElement(ns, baseUrl, $"/{slug}", "weekly", "0.8"));
            }

            // Pages
            var pages = await _context.Pages
                .Select(p => new { p.Slug, p.UpdatedAt })
                .ToListAsync();

            foreach (var page in pages)
            {
                urlElements.Add(CreateUrlElement(ns, baseUrl, $"/page/{page.Slug}", "monthly", "0.6", page.UpdatedAt));
            }

            var sitemap = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement(ns + "urlset", urlElements)
            );

            return Content(sitemap.Declaration + Environment.NewLine + sitemap.ToString(), "application/xml", Encoding.UTF8);
        }

        private static XElement CreateUrlElement(XNamespace ns, string baseUrl, string path,
            string changeFreq, string priority, DateTime? lastMod = null)
        {
            var element = new XElement(ns + "url",
                new XElement(ns + "loc", baseUrl + path),
                new XElement(ns + "changefreq", changeFreq),
                new XElement(ns + "priority", priority)
            );

            if (lastMod.HasValue)
            {
                element.Add(new XElement(ns + "lastmod", lastMod.Value.ToString("yyyy-MM-dd")));
            }

            return element;
        }
    }
}
