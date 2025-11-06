namespace SearchMvp.Api.Models;

public class Product
{
    public int Id { get; set; } // PK (DB)
    public string Sku { get; set; } = "";
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}