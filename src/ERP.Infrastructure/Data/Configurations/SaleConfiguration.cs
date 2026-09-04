namespace ERP.Infrastructure.Data.Configurations;

using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.SalesNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Total)
            .HasPrecision(18, 2);

        builder.Property(x => x.Discount)
            .HasPrecision(18, 2);

        builder.HasIndex(x => new { x.CompanyId, x.SalesNumber }).IsUnique();
        builder.HasIndex(x => new { x.CompanyId, x.SalesAt });
        builder.HasIndex(x => x.Status);

        builder.HasOne(x => x.Customer)
            .WithMany(c => c.Sales)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.PriceTable)
            .WithMany(pt => pt.Sales)
            .HasForeignKey(x => x.PriceTableId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
