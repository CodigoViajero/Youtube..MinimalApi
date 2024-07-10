using Microsoft.EntityFrameworkCore;
using Minimal.Api.Data.EntityConfig;
using Minimal.Api.Model;

namespace Minimal.Api.Data;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductEntityType());
    }

    public DbSet<Product> Products { get; set; }
}