using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minimal.Api.Model;

namespace Minimal.Api.Data.EntityConfig;

public class ProductEntityType : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Name)
            .IsRequired(true)
            .HasMaxLength(50);

        builder.Property(ci => ci.Price)
            .HasPrecision(18, 4)
            .IsRequired(true);

        builder.Property(ci => ci.CreatedAt)
            .HasDefaultValue(DateTime.Now);
    }
}