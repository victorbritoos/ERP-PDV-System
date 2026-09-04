namespace ERP.Infrastructure.Data.Configurations;

using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AccountsPayableConfiguration : IEntityTypeConfiguration<AccountsPayable>
{
    public void Configure(EntityTypeBuilder<AccountsPayable> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.HasIndex(x => new { x.CompanyId, x.SupplierId, x.DueDate });
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.DueDate);

        builder.HasOne(x => x.Supplier)
            .WithMany(s => s.Accounts)
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
