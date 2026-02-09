using System;
using System.Collections.Generic;

namespace TelefonOzellikleri.Models;

public partial class Series
{
    public int Id { get; set; }

    public int BrandId { get; set; }

    public string SeriesName { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<Smartphone> Smartphones { get; set; } = new List<Smartphone>();
}
