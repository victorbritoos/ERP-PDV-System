namespace ERP.Infrastructure.Data.Configurations;

using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Document)
            .HasMaxLength(20);

        builder.Property(x => x.Email)
            .HasMaxLength(100);

        builder.Property(x => x.CreditLimit)
            .HasPrecision(18, 2);

        builder.HasIndex(x => new { x.CompanyId, x.Document }).IsUnique();
        builder.HasIndex(x => new { x.CompanyId, x.Name });
        builder.HasIndex(x => x.IsActive);

        builder.HasOne(x => x.DefaultPriceTable)
            .WithMany()
            .HasForeignKey(x => x.DefaultPriceTableId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
