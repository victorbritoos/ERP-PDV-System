namespace ERP.Infrastructure.Data.Configurations;

using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductPriceConfiguration : IEntityTypeConfiguration<ProductPrice>
{
    public void Configure(EntityTypeBuilder<ProductPrice> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        // Índices
        builder.HasIndex(x => new { x.CompanyId, x.ProductId, x.PriceTableId, x.EffectiveFrom })
            .IsUnique()
            .HasDatabaseName("IX_ProductPrice_Unique");
        builder.HasIndex(x => new { x.ProductId, x.EffectiveFrom, x.EffectiveTo });

        // Relacionamentos
        builder.HasOne(x => x.Product)
            .WithMany(p => p.Prices)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.PriceTable)
            .WithMany(pt => pt.ProductPrices)
            .HasForeignKey(x => x.PriceTableId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
