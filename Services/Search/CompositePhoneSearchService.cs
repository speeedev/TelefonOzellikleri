using TelefonOzellikleri.Models.ViewModels;

namespace TelefonOzellikleri.Services.Search;

/// <summary>
/// Combines Elasticsearch and DB search: uses Elasticsearch when available, falls back to DB.
/// </summary>
public class CompositePhoneSearchService : IPhoneSearchService
{
    private readonly ElasticsearchPhoneSearchService _elasticsearch;
    private readonly DbPhoneSearchService _db;
    private readonly ILogger<CompositePhoneSearchService> _logger;

    public bool IsAvailable => _elasticsearch.IsAvailable || true;

    public CompositePhoneSearchService(
        ElasticsearchPhoneSearchService elasticsearch,
        DbPhoneSearchService db,
        ILogger<CompositePhoneSearchService> logger)
    {
        _elasticsearch = elasticsearch;
        _db = db;
        _logger = logger;
    }

    public async Task<IReadOnlyList<SearchResultItem>> SearchAsync(string query, int maxResults = 24, CancellationToken cancellationToken = default)
    {
        if (_elasticsearch.IsAvailable)
        {
            var results = await _elasticsearch.SearchAsync(query, maxResults, cancellationToken);
            if (results.Count > 0)
                return results;
        }

        return await _db.SearchAsync(query, maxResults, cancellationToken);
    }

    public async Task IndexAllAsync(CancellationToken cancellationToken = default)
    {
        await _elasticsearch.IndexAllAsync(cancellationToken);
    }

    public async Task IndexAsync(int phoneId, CancellationToken cancellationToken = default)
    {
        await _elasticsearch.IndexAsync(phoneId, cancellationToken);
    }

    public async Task DeleteFromIndexAsync(string slug, CancellationToken cancellationToken = default)
    {
        await _elasticsearch.DeleteFromIndexAsync(slug, cancellationToken);
    }
}
