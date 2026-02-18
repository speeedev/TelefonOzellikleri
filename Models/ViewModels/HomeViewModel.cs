using TelefonOzellikleri.Models;

namespace TelefonOzellikleri.Models.ViewModels
{
    public class HomeViewModel
    {
        public string SiteTitle { get; set; } = string.Empty;
        public string SiteDescription { get; set; } = string.Empty;
        public string? FooterText { get; set; }
        public List<FeedItem> News { get; set; } = new();
        public List<LatestPhoneItem> LatestPhones { get; set; } = new();
    }

    public class LatestPhoneItem
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
        public DateOnly? ReleaseDate { get; set; }
    }
}