namespace ERP.Infrastructure.Data.Configurations;

using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CashRegisterConfiguration : IEntityTypeConfiguration<CashRegister>
{
    public void Configure(EntityTypeBuilder<CashRegister> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(x => new { x.CompanyId, x.Name }).IsUnique();
    }
}

public class CashRegisterSessionConfiguration : IEntityTypeConfiguration<CashRegisterSession>
{
    public void Configure(EntityTypeBuilder<CashRegisterSession> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.InitialBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.FinalBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.BalanceDifference)
            .HasPrecision(18, 2);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(x => new { x.CompanyId, x.OpenedAt });
        builder.HasIndex(x => x.Status);

        builder.HasOne(x => x.CashRegister)
            .WithMany(cr => cr.Sessions)
            .HasForeignKey(x => x.CashRegisterId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class CashRegisterMovementConfiguration : IEntityTypeConfiguration<CashRegisterMovement>
{
    public void Configure(EntityTypeBuilder<CashRegisterMovement> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(x => new { x.CompanyId, x.RecordedAt });
        builder.HasIndex(x => x.MovementType);

        builder.HasOne(x => x.CashRegisterSession)
            .WithMany(s => s.Movements)
            .HasForeignKey(x => x.CashRegisterSessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
