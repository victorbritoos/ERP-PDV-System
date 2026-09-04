namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Histórico de alterações de preço
/// </summary>
public class PriceHistory : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid PriceTableId { get; set; }
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public string? ChangeReason { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public virtual ProductPrice? ProductPrice { get; set; }
}
