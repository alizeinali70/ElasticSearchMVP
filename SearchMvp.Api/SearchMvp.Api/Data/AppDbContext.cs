using Microsoft.EntityFrameworkCore;
using SearchMvp.Api.Models;

namespace SearchMvp.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> opts) : DbContext(opts)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Product>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Sku).HasMaxLength(50).IsRequired();
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.Description).HasMaxLength(2000);
            e.Property(x => x.Price).HasColumnType("decimal(18,2)");
            e.HasIndex(x => x.Sku).IsUnique();
            e.HasIndex(x => x.Name);
        });
    }
}