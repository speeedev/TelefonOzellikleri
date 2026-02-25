using System.Xml.Linq;
using Microsoft.Extensions.Caching.Memory;
using TelefonOzellikleri.Cache;
using TelefonOzellikleri.Models;

namespace TelefonOzellikleri.Services;

public class FeedService
{
    private const int NewsCacheMinutes = 60;

    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<FeedService> _logger;

    private const string FeedUrl = "https://feeds.feedburner.com/tamindir/stream";

    public FeedService(HttpClient httpClient, IMemoryCache cache, ILogger<FeedService> logger)
    {
        _httpClient = httpClient;
        _cache = cache;
        _logger = logger;
    }

    public async Task<List<FeedItem>> GetNewsAsync()
    {
        return await _cache.GetOrCreateAsync(CacheKeys.NewsFeed, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(NewsCacheMinutes);

            try
            {
                var response = await _httpClient.GetAsync(FeedUrl);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();
                var doc = XDocument.Load(stream);

                var items = doc.Descendants("item")
                    .Where(item => item.Element("category")?.Value == "Mobil")
                    .Take(20)
                    .Select(item => new FeedItem
                    {
                        Title = System.Net.WebUtility.HtmlDecode(item.Element("title")?.Value ?? "Başlık Yok"),
                        Description = CleanDescription(item.Element("description")?.Value ?? "Özet Yok"),
                        Link = item.Element("link")?.Value ?? "#",
                        ImageUrl = item.Element("enclosure")?.Attribute("url")?.Value ?? string.Empty,
                    })
                    .ToList();

                _logger.LogInformation("News feed loaded and cached: {Count} items", items.Count);
                return items;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch news feed, returning empty list");
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1); // Short TTL on error
                return new List<FeedItem>();
            }
        }) ?? new List<FeedItem>();
    }

    private static string CleanDescription(string description)
    {
        if (string.IsNullOrEmpty(description))
            return string.Empty;

        // HTML entity decode
        var decoded = System.Net.WebUtility.HtmlDecode(description);

        // Clean characters defined in CDATA
        decoded = System.Text.RegularExpressions.Regex.Replace(decoded, @"<!\[CDATA\[(.*?)\]\]>", "$1");

        // Remove HTML tags
        decoded = System.Text.RegularExpressions.Regex.Replace(decoded, "<[^>]+>", "");

        // Clean extra whitespace
        decoded = System.Text.RegularExpressions.Regex.Replace(decoded, @"\s+", " ").Trim();

        return decoded;
    }
}
