using Microsoft.EntityFrameworkCore;

namespace TelefonOzellikleri.Data;

public static class SlugUniquenessExtensions
{
    public static async Task<bool> IsSlugAvailableAsync(
        this TelefonOzellikleriDbContext context,
        string slug,
        int? excludePageId = null,
        int? excludeSmartphoneId = null,
        int? excludeBrandId = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return false;

        var inPages = excludePageId.HasValue
            ? await context.Pages.AnyAsync(p => p.Slug == slug && p.Id != excludePageId.Value, cancellationToken)
            : await context.Pages.AnyAsync(p => p.Slug == slug, cancellationToken);
        if (inPages) return false;

        var inSmartphones = excludeSmartphoneId.HasValue
            ? await context.Smartphones.AnyAsync(s => s.Slug == slug && s.Id != excludeSmartphoneId.Value, cancellationToken)
            : await context.Smartphones.AnyAsync(s => s.Slug == slug, cancellationToken);
        if (inSmartphones) return false;

        var inBrands = excludeBrandId.HasValue
            ? await context.Brands.AnyAsync(b => b.Slug == slug && b.Id != excludeBrandId.Value, cancellationToken)
            : await context.Brands.AnyAsync(b => b.Slug == slug, cancellationToken);
        if (inBrands) return false;

        return true;
    }
}
