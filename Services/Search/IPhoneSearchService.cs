using TelefonOzellikleri.Models.ViewModels;

namespace TelefonOzellikleri.Services.Search;

public interface IPhoneSearchService
{
    Task<IReadOnlyList<SearchResultItem>> SearchAsync(string query, int maxResults = 24, CancellationToken cancellationToken = default);
    Task IndexAllAsync(CancellationToken cancellationToken = default);
    Task IndexAsync(int phoneId, CancellationToken cancellationToken = default);
    Task DeleteFromIndexAsync(string slug, CancellationToken cancellationToken = default);
    bool IsAvailable { get; }
}
