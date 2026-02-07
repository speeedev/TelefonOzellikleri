using System;
using System.Collections.Generic;

namespace TelefonOzellikleri.Models;

public partial class Brand
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? LogoUrl { get; set; }
}
