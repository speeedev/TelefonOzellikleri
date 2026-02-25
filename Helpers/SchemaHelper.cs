using System.Text.Json;

namespace TelefonOzellikleri.Helpers;

/// <summary>
/// Schema.org JSON-LD yapısal veri oluşturma yardımcıları.
/// </summary>
public static class SchemaHelper
{
    /// <summary>
    /// JSON string içindeki özel karakterleri escape eder.
    /// </summary>
    public static string EscapeJsonString(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;
        return JsonSerializer.Serialize(value);
    }
}
