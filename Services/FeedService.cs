using System.Xml.Linq;
using TelefonOzellikleri.Models;

namespace TelefonOzellikleri.Services;

public class FeedService
{
    private readonly HttpClient _httpClient;
    private const string FeedUrl = "https://feeds.feedburner.com/tamindir/stream";
    private const int CacheDurationMinutes = 60; // Cache time (60 minutes)
    private List<FeedItem>? _cachedItems;
    private DateTime _cacheTime;

    public FeedService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _cacheTime = DateTime.MinValue;
    }

    public async Task<List<FeedItem>> GetNewsAsync()
    {
        // Cache kontrolü
        if (_cachedItems != null && DateTime.UtcNow < _cacheTime.AddMinutes(CacheDurationMinutes))
        {
            return _cachedItems;
        }

        try
        {
            var response = await _httpClient.GetAsync(FeedUrl);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            var doc = XDocument.Load(stream);

            _cachedItems = doc.Descendants("item")
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

            _cacheTime = DateTime.UtcNow;
            return _cachedItems;
        }
        catch
        {
            return new List<FeedItem>();
        }
    }

    private static string CleanDescription(string description)
    {
        if (string.IsNullOrEmpty(description))
            return string.Empty;

        // HTML entity decode
        var decoded = System.Net.WebUtility.HtmlDecode(description);

        // CDATA'da tanımlanan karakterleri temizle
        decoded = System.Text.RegularExpressions.Regex.Replace(decoded, @"<!\[CDATA\[(.*?)\]\]>", "$1");

        // HTML tag'lerini kaldır
        decoded = System.Text.RegularExpressions.Regex.Replace(decoded, "<[^>]+>", "");

        // Extra boşlukları temizle
        decoded = System.Text.RegularExpressions.Regex.Replace(decoded, @"\s+", " ").Trim();

        return decoded;
    }
}
