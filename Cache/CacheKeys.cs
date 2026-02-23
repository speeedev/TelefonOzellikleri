namespace TelefonOzellikleri.Cache;

/// <summary>
/// Centralized cache keys.
/// </summary>
public static class CacheKeys
{
    public const string HomePage = "homepage";

    public static string PhoneDetail(string slug) =>
        $"phone_detail_{slug.ToLowerInvariant()}";

    public static string PageDetail(string slug) =>
        $"page_detail_{slug.ToLowerInvariant()}";
}
