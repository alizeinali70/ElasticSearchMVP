using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SearchMvp.Api.Data;
using SearchMvp.Api.Search;

namespace SearchMvp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IProductSearch _search;

    public ProductsController(AppDbContext db, IProductSearch search)
    {
        _db = db;
        _search = search;
    }

    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken ct)
    {
        var products = await _db.Products
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .ToListAsync(ct);

        return Ok(products);
    }

    // GET: api/products/search?q=keyboard&size=10
    [HttpGet("search")]
    public async Task<IActionResult> SearchAsync([FromQuery] string q, [FromQuery] int size = 10, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest("Query text is required.");

        var results = await _search.SearchAsync(q, Math.Clamp(size, 1, 100), ct);

        return Ok(results);
    }

    // POST: api/products/reindex
    [HttpPost("reindex")]
    public async Task<IActionResult> ReindexAsync(CancellationToken ct)
    {
        var products = await _db.Products.AsNoTracking().ToListAsync(ct);
        await _search.IndexManyAsync(products, ct);

        return Ok(new { count = products.Count });
    }
}