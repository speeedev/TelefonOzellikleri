using System;
using System.Collections.Generic;

namespace TelefonOzellikleri.Models;

public partial class Page
{
    public int Id { get; set; }

    public string Slug { get; set; } = null!;

    public string? PageTitle { get; set; }

    public string? PageDescription { get; set; }

    public string? Content { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
