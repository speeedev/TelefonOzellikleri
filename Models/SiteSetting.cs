using System;
using System.Collections.Generic;

namespace TelefonOzellikleri.Models;

public partial class SiteSetting
{
    public int Id { get; set; }

    public string? SiteName { get; set; }

    public string? SiteTitle { get; set; }

    public string? SiteDescription { get; set; }

    public string? LogoUrl { get; set; }

    public string? FaviconUrl { get; set; }

    public string? XUrl { get; set; }

    public string? InstagramUrl { get; set; }

    public string? YoutubeUrl { get; set; }

    public string? FooterText { get; set; }

    public bool? IsMaintenanceMode { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
