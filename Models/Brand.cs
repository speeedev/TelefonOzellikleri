using System;
using System.Collections.Generic;

namespace TelefonOzellikleri.Models;

public partial class Brand
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? LogoUrl { get; set; }

    public virtual ICollection<Series> Series { get; set; } = new List<Series>();

    public virtual ICollection<Smartphone> Smartphones { get; set; } = new List<Smartphone>();
}
