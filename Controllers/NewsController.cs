using Microsoft.AspNetCore.Mvc;
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
        var news = await _feedService.GetNewsAsync();
        return View(news);
    }
}
