namespace TelefonOzellikleri.Models.Elasticsearch;

/// <summary>
/// Elasticsearch index document for smartphone search.
/// </summary>
public class SmartphoneSearchDocument
{
    public int Id { get; set; }
    public string Slug { get; set; } = null!;
    public string ModelName { get; set; } = null!;
    public string BrandName { get; set; } = null!;
    public string? BrandLogoUrl { get; set; }
    public string? MainImageUrl { get; set; }
    public string? Chipset { get; set; }
    public int? BatteryCapacity { get; set; }
    public string? Cam1Res { get; set; }
    public decimal? ScreenSize { get; set; }
}
