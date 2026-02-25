namespace TelefonOzellikleri.Helpers;

/// <summary>
/// SEO uyumlu title ve description için yardımcı metodlar.
/// Google: Title ~60 karakter, Description ~155-160 karakter gösterir.
/// </summary>
public static class SeoHelper
{
    public const int MaxTitleLength = 60;
    public const int MaxDescriptionLength = 160;

    /// <summary>
    /// SEO uyumlu title oluşturur (max 60 karakter).
    /// Kelime ortasında kesmez.
    /// </summary>
    public static string TruncateTitle(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        return TruncateAtWord(value.Trim(), MaxTitleLength);
    }

    /// <summary>
    /// SEO uyumlu meta description oluşturur (max 160 karakter).
    /// Kelime ortasında kesmez.
    /// </summary>
    public static string TruncateDescription(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        return TruncateAtWord(value.Trim(), MaxDescriptionLength);
    }

    private static string TruncateAtWord(string value, int maxLength)
    {
        if (value.Length <= maxLength)
            return value;

        var truncated = value[..maxLength];
        var lastSpace = truncated.LastIndexOf(' ');

        if (lastSpace > maxLength * 0.6)
            return truncated[..lastSpace].Trim();

        return truncated.Trim();
    }
}
