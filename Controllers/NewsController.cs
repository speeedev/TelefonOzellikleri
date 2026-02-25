using Microsoft.AspNetCore.Mvc;
using TelefonOzellikleri.Helpers;
using TelefonOzellikleri.Services;

namespace TelefonOzellikleri.Controllers;

public class NewsController : Controller
{
    private readonly FeedService _feedService;

    public NewsController(FeedService feedService)
    {
        _feedService = feedService;
    }

    [Route("akilli-telefon-haberleri")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = SeoHelper.TruncateTitle("Akıllı Telefon Haberleri - Son Gelişmeler");
        ViewData["Description"] = SeoHelper.TruncateDescription("En güncel akıllı telefon haberleri, lansmanlar ve sektör gelişmeleri. Mobil teknoloji dünyasından son dakika haberleri.");
        var news = await _feedService.GetNewsAsync();
        return View(news);
    }
}
