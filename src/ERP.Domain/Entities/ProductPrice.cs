namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Entidade de preço de produto (CORE DO MOTOR DE PREÇOS)
/// Um produto pode ter N preços diferentes (uma por tabela de preço)
/// </summary>
public class ProductPrice : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid PriceTableId { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;
    public DateTime? EffectiveTo { get; set; } = null; // null = sem data final

    // Relacionamentos
    public virtual Product? Product { get; set; }
    public virtual PriceTable? PriceTable { get; set; }
    public virtual ICollection<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();
}
