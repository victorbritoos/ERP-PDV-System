namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Tabela de preço padrão por cliente
/// </summary>
public class CustomerPriceTable : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Guid PriceTableId { get; set; }
    public int Priority { get; set; } = 0;
    public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;
    public DateTime? EffectiveTo { get; set; } = null;

    // Relacionamentos
    public virtual Customer? Customer { get; set; }
    public virtual PriceTable? PriceTable { get; set; }
}
