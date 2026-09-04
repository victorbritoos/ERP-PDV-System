namespace ERP.Infrastructure.Data.Configurations;

using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Sku)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Barcode)
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.CostPrice)
            .HasPrecision(18, 2);

        builder.Property(x => x.CurrentStock)
            .HasPrecision(18, 4);

        builder.Property(x => x.StockMinimum)
            .HasPrecision(18, 4);

        builder.Property(x => x.StockMaximum)
            .HasPrecision(18, 4);

        // Índices
        builder.HasIndex(x => new { x.CompanyId, x.Sku }).IsUnique().HasDatabaseName("IX_Product_Company_Sku");
        builder.HasIndex(x => new { x.CompanyId, x.Barcode }).IsUnique().HasDatabaseName("IX_Product_Company_Barcode");
        builder.HasIndex(x => x.CategoryId);
        builder.HasIndex(x => x.IsActive);

        // Relacionamentos
        builder.HasOne(x => x.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(x => x.BrandId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Supplier)
            .WithMany(s => s.Products)
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
