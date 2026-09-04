namespace ERP.Infrastructure.Data.Configurations;

using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PriceTableConfiguration : IEntityTypeConfiguration<PriceTable>
{
    public void Configure(EntityTypeBuilder<PriceTable> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.IsDefault)
            .HasDefaultValue(false);

        builder.HasIndex(x => new { x.CompanyId, x.Name }).IsUnique();
        builder.HasIndex(x => new { x.CompanyId, x.IsActive });
        builder.HasIndex(x => new { x.CompanyId, x.Priority });
    }
}
