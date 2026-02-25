using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TelefonOzellikleri.Configuration;
using TelefonOzellikleri.Data;
using TelefonOzellikleri.Models.Elasticsearch;
using TelefonOzellikleri.Models.ViewModels;

namespace TelefonOzellikleri.Services.Search;

public class ElasticsearchPhoneSearchService : IPhoneSearchService
{
    private readonly ElasticsearchClient _client;
    private readonly string _indexName;
    private readonly TelefonOzellikleriDbContext _context;
    private readonly ILogger<ElasticsearchPhoneSearchService> _logger;

    public bool IsAvailable { get; private set; }

    public ElasticsearchPhoneSearchService(
        ElasticsearchClient client,
        IOptions<ElasticsearchSettings> options,
        TelefonOzellikleriDbContext context,
        ILogger<ElasticsearchPhoneSearchService> logger)
    {
        var settings = options.Value;
        _client = client;
        _indexName = settings.IndexName;
        _context = context;
        _logger = logger;
        IsAvailable = settings.IsConfigured;
    }

    public async Task<IReadOnlyList<SearchResultItem>> SearchAsync(string query, int maxResults = 24, CancellationToken cancellationToken = default)
    {
        if (!IsAvailable || string.IsNullOrWhiteSpace(query))
            return Array.Empty<SearchResultItem>();

        try
        {
            var response = await _client.SearchAsync<SmartphoneSearchDocument>(s => s
                .Index(_indexName)
                .Size(maxResults)
                .Query(q => q
                    .MultiMatch(mm => mm
                        .Fields(new[] { "modelName", "brandName" })
                        .Query(query)
                        .Fuzziness(new Elastic.Clients.Elasticsearch.Fuzziness("AUTO"))
                    )
                ),
                cancellationToken);

            if (!response.IsValidResponse)
            {
                _logger.LogWarning("Elasticsearch search failed: {Debug}", response.DebugInformation);
                return Array.Empty<SearchResultItem>();
            }

            return response.Documents.Select(MapToResultItem).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Elasticsearch search error for '{Query}', falling back to DB", query);
            return Array.Empty<SearchResultItem>();
        }
    }

    public async Task IndexAllAsync(CancellationToken cancellationToken = default)
    {
        if (!IsAvailable) return;

        try
        {
            var phones = await _context.Smartphones
                .AsNoTracking()
                .Join(_context.Brands,
                    phone => phone.BrandId,
                    brand => brand.Id,
                    (phone, brand) => new SmartphoneSearchDocument
                    {
                        Id = phone.Id,
                        Slug = phone.Slug,
                        ModelName = phone.ModelName,
                        BrandName = brand.Name,
                        BrandLogoUrl = brand.LogoUrl,
                        MainImageUrl = phone.MainImageUrl,
                        Chipset = phone.Chipset,
                        BatteryCapacity = phone.BatteryCapacity,
                        Cam1Res = phone.Cam1Res,
                        ScreenSize = phone.ScreenSize
                    })
                .ToListAsync(cancellationToken);

            foreach (var doc in phones)
            {
                var response = await _client.IndexAsync(doc, i => i
                    .Index(_indexName)
                    .Id(doc.Slug),
                    cancellationToken);
                if (!response.IsValidResponse)
                    _logger.LogWarning("Failed to index {Slug}: {Debug}", doc.Slug, response.DebugInformation);
            }

            _logger.LogInformation("Indexed {Count} smartphones to Elasticsearch", phones.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to index smartphones to Elasticsearch");
            IsAvailable = false;
        }
    }

    public async Task IndexAsync(int phoneId, CancellationToken cancellationToken = default)
    {
        if (!IsAvailable) return;

        try
        {
            var doc = await _context.Smartphones
                .AsNoTracking()
                .Where(p => p.Id == phoneId)
                .Join(_context.Brands,
                    phone => phone.BrandId,
                    brand => brand.Id,
                    (phone, brand) => new SmartphoneSearchDocument
                    {
                        Id = phone.Id,
                        Slug = phone.Slug,
                        ModelName = phone.ModelName,
                        BrandName = brand.Name,
                        BrandLogoUrl = brand.LogoUrl,
                        MainImageUrl = phone.MainImageUrl,
                        Chipset = phone.Chipset,
                        BatteryCapacity = phone.BatteryCapacity,
                        Cam1Res = phone.Cam1Res,
                        ScreenSize = phone.ScreenSize
                    })
                .FirstOrDefaultAsync(cancellationToken);

            if (doc == null) return;

            var response = await _client.IndexAsync(doc, i => i
                .Index(_indexName)
                .Id(doc.Slug),
                cancellationToken);

            if (response.IsValidResponse)
                _logger.LogDebug("Indexed phone {Slug} to Elasticsearch", doc.Slug);
            else
                _logger.LogWarning("Elasticsearch index failed for {Slug}: {Debug}", doc.Slug, response.DebugInformation);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to index phone {PhoneId} to Elasticsearch", phoneId);
        }
    }

    public async Task DeleteFromIndexAsync(string slug, CancellationToken cancellationToken = default)
    {
        if (!IsAvailable || string.IsNullOrWhiteSpace(slug)) return;

        try
        {
            var response = await _client.DeleteAsync(_indexName, slug, cancellationToken);
            if (response.IsValidResponse)
                _logger.LogDebug("Deleted phone {Slug} from Elasticsearch index", slug);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete phone {Slug} from Elasticsearch", slug);
        }
    }

    private static SearchResultItem MapToResultItem(SmartphoneSearchDocument doc) => new()
    {
        Slug = doc.Slug,
        ModelName = doc.ModelName,
        BrandName = doc.BrandName,
        BrandLogoUrl = doc.BrandLogoUrl,
        MainImageUrl = doc.MainImageUrl,
        Chipset = doc.Chipset,
        BatteryCapacity = doc.BatteryCapacity,
        Cam1Res = doc.Cam1Res,
        ScreenSize = doc.ScreenSize
    };
}
