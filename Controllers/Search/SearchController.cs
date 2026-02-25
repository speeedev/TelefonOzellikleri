using Microsoft.AspNetCore.Mvc;
using TelefonOzellikleri.Helpers;
using TelefonOzellikleri.Models.ViewModels;
using TelefonOzellikleri.Services.Search;

namespace TelefonOzellikleri.Controllers.Search;

[Route("search")]
public class SearchController : Controller
{
    private const int MaxSearchResults = 24;

    private readonly ILogger<SearchController> _logger;
    private readonly IPhoneSearchService _searchService;

    public SearchController(ILogger<SearchController> logger, IPhoneSearchService searchService)
    {
        _logger = logger;
        _searchService = searchService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? q)
    {
        ViewData["Title"] = SeoHelper.TruncateTitle("Telefon Ara - Akıllı Telefon Özellikleri");
        ViewData["Description"] = SeoHelper.TruncateDescription("Akıllı telefon modellerini marka ve model adına göre arayın. Binlerce telefon özelliğini karşılaştırın.");

        var viewModel = new SearchViewModel
        {
            Query = q?.Trim() ?? string.Empty,
            Results = new List<SearchResultItem>()
        };

        if (string.IsNullOrWhiteSpace(viewModel.Query))
        {
            return View(viewModel);
        }

        var results = await _searchService.SearchAsync(viewModel.Query, MaxSearchResults);
        viewModel.Results = results.ToList();
        _logger.LogInformation("Search for '{Query}' returned {Count} results", viewModel.Query, results.Count);

        return View(viewModel);
    }
}
