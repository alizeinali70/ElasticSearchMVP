namespace SearchMvp.Api.Search;

public record EsProduct(
    int Id,
    string Sku,
    string Name,
    string? Description,
    decimal Price
);