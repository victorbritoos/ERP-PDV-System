namespace ERP.Infrastructure.Data.Configurations;

using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AccountsReceivableConfiguration : IEntityTypeConfiguration<AccountsReceivable>
{
    public void Configure(EntityTypeBuilder<AccountsReceivable> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.HasIndex(x => new { x.CompanyId, x.CustomerId, x.DueDate });
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.DueDate);

        builder.HasOne(x => x.Customer)
            .WithMany(c => c.AccountsReceivable)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
