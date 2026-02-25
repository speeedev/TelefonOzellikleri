using Microsoft.EntityFrameworkCore;
using TelefonOzellikleri.Data;
using TelefonOzellikleri.Models.ViewModels;

namespace TelefonOzellikleri.Services.Search;

/// <summary>
/// Database-based phone search fallback when Elasticsearch is unavailable.
/// </summary>
public class DbPhoneSearchService : IPhoneSearchService
{
    private readonly TelefonOzellikleriDbContext _context;

    public bool IsAvailable => true;

    public DbPhoneSearchService(TelefonOzellikleriDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<SearchResultItem>> SearchAsync(string query, int maxResults = 24, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Array.Empty<SearchResultItem>();

        var searchTerm = query.Trim();
        return await _context.Smartphones
            .AsNoTracking()
            .Join(_context.Brands,
                phone => phone.BrandId,
                brand => brand.Id,
                (phone, brand) => new { phone, brand })
            .Where(x => x.phone.ModelName.Contains(searchTerm) || x.brand.Name.Contains(searchTerm))
            .Select(x => new SearchResultItem
                {
                    Slug = x.phone.Slug,
                    ModelName = x.phone.ModelName,
                    BrandName = x.brand.Name,
                    BrandLogoUrl = x.brand.LogoUrl,
                    MainImageUrl = x.phone.MainImageUrl,
                    Chipset = x.phone.Chipset,
                    BatteryCapacity = x.phone.BatteryCapacity,
                    Cam1Res = x.phone.Cam1Res,
                    ScreenSize = x.phone.ScreenSize
                })
            .Take(maxResults)
            .ToListAsync(cancellationToken);
    }

    public Task IndexAllAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task IndexAsync(int phoneId, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task DeleteFromIndexAsync(string slug, CancellationToken cancellationToken = default) => Task.CompletedTask;
}
