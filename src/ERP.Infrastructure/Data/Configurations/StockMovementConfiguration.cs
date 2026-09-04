namespace ERP.Infrastructure.Data.Configurations;

using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Quantity)
            .HasPrecision(18, 4);

        builder.HasIndex(x => new { x.CompanyId, x.ProductId, x.RecordedAt });
        builder.HasIndex(x => new { x.ReferencedSaleId });
        builder.HasIndex(x => x.MovementType);

        builder.HasOne(x => x.Product)
            .WithMany(p => p.StockMovements)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
