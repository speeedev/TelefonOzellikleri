using Microsoft.AspNetCore.Mvc;

namespace TelefonOzellikleri.Controllers
{
    public class RobotsController : Controller
    {
        [Route("robots.txt")]
        [ResponseCache(Duration = 86400)]
        public IActionResult Index()
        {
            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            var robotsTxt = $"""
                User-agent: *
                Allow: /
                Disallow: /Error/

                Sitemap: {baseUrl}/sitemap.xml
                """;

            return Content(robotsTxt, "text/plain");
        }
    }
}
