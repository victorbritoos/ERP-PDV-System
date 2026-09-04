namespace ERP.Infrastructure.Data.Configurations;

using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.EntityName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.EntityId)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => new { x.CompanyId, x.Timestamp });
        builder.HasIndex(x => x.EntityName);
        builder.HasIndex(x => x.Action);
        builder.HasIndex(x => x.UserId);
    }
}
