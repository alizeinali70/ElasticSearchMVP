using SearchMvp.Api.Models;

namespace SearchMvp.Api.Search;

public interface IProductSearch
{
    Task IndexManyAsync(IEnumerable<Product> products, CancellationToken ct);
    Task<IEnumerable<EsProduct>> SearchAsync(string query, int size, CancellationToken ct);
    Task EnsureIndexAsync(CancellationToken ct);
}