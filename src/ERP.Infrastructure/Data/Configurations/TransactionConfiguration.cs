namespace ERP.Infrastructure.Data.Configurations;

using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SalePaymentConfiguration : IEntityTypeConfiguration<SalePayment>
{
    public void Configure(EntityTypeBuilder<SalePayment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(100);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(x => new { x.CompanyId, x.ProcessedAt });
        builder.HasIndex(x => x.Status);

        builder.HasOne(x => x.Sale)
            .WithMany(s => s.Payments)
            .HasForeignKey(x => x.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.PaymentMethod)
            .WithMany()
            .HasForeignKey(x => x.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class SaleCancellationConfiguration : IEntityTypeConfiguration<SaleCancellation>
{
    public void Configure(EntityTypeBuilder<SaleCancellation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CancellationReason)
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(x => new { x.CompanyId, x.CancelledAt });

        builder.HasOne(x => x.Sale)
            .WithOne(s => s.Cancellation)
            .HasForeignKey<SaleCancellation>(x => x.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class InventoryAdjustmentConfiguration : IEntityTypeConfiguration<InventoryAdjustment>
{
    public void Configure(EntityTypeBuilder<InventoryAdjustment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.SystemQuantity)
            .HasPrecision(18, 4);

        builder.Property(x => x.PhysicalQuantity)
            .HasPrecision(18, 4);

        builder.Property(x => x.Difference)
            .HasPrecision(18, 4);

        builder.Property(x => x.AdjustmentReason)
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(x => new { x.CompanyId, x.CreatedAt });

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Supplier)
            .HasMaxLength(200);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(x => new { x.CompanyId, x.DueDate });
        builder.HasIndex(x => x.Category);
    }
}
