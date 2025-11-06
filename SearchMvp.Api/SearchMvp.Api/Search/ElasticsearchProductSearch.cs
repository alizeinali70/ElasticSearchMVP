using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;
using SearchMvp.Api.Data;
using SearchMvp.Api.Models;

namespace SearchMvp.Api.Search;

public sealed class ElasticsearchProductSearch(
    ElasticsearchClient client,
    AppDbContext db,
    IOptions<EsSettings> opts
) : IProductSearch
{
    private static readonly string[] SearchFields = ["name^3", "description", "sku^2"];
    private readonly ElasticsearchClient _client = client;
    private readonly AppDbContext _db = db;
    private readonly EsSettings _settings = opts.Value;

    private string IndexName => $"{_settings.IndexPrefix}-products";

    public async Task EnsureIndexAsync(CancellationToken ct)
    {
        var exists = await _client.Indices.ExistsAsync(IndexName, ct);

        if (exists.Exists)
            return;

        var create = await _client.Indices.CreateAsync(IndexName, d => d
            .Mappings(m => m
                .Properties<EsProduct>(ps => ps
                    .Text(p => p.Name)
                    .Text(p => p.Description)
                    .Keyword(p => p.Sku)
                    .DoubleNumber(p => p.Price)
                    .IntegerNumber(p => p.Id)
                )
            ), ct);


        if (!create.IsValidResponse)
        {
            Console.WriteLine($"Index creation failed: {create.DebugInformation}");

            throw new Exception($"Failed to create index: {create.ElasticsearchServerError?.Error.Reason}");
        }
    }

    public async Task IndexManyAsync(IEnumerable<Product> products, CancellationToken ct)
    {
        await EnsureIndexAsync(ct);

        var docs = products.Select(p => new EsProduct(p.Id, p.Sku, p.Name, p.Description, p.Price));
        var bulk = await _client.BulkAsync(b => { b.IndexMany(docs, (descriptor, d) => descriptor.Index(IndexName).Id(d.Id.ToString())); }, ct);

        if (!bulk.IsValidResponse)
            throw new Exception($"Bulk index failed: {bulk.ElasticsearchServerError?.Error.Reason}");

        await _client.Indices.RefreshAsync(IndexName, ct);
    }

    public async Task<IEnumerable<EsProduct>> SearchAsync(string query, int size, CancellationToken ct)
    {
        await EnsureIndexAsync(ct);

        var resp = await _client.SearchAsync<EsProduct>(s => s
            .Index(IndexName)
            .Size(size)
            .Query(q => q
                .MultiMatch(mm => mm
                    .Query(query)
                    .Fields(SearchFields)
                    .MinimumShouldMatch("2<-25%")
                )
            ), ct);

        if (!resp.IsValidResponse)
        {
            Console.WriteLine($"Search failed: {resp.DebugInformation}");

            return Enumerable.Empty<EsProduct>();
        }

        return resp.Hits
            .Select(h => h.Source)
            .Where(s => s is not null)!;
    }
}