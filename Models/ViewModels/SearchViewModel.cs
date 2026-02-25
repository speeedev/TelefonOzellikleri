namespace TelefonOzellikleri.Models.ViewModels;

public class SearchViewModel
{
    public string Query { get; set; } = string.Empty;
    public List<SearchResultItem> Results { get; set; } = new();
}

public class SearchResultItem
{
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
