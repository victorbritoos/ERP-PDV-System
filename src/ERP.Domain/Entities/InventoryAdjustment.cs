namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Ajuste de inventário (balanço de estoque)
/// </summary>
public class InventoryAdjustment : BaseEntity
{
    public Guid ProductId { get; set; }
    public decimal SystemQuantity { get; set; };  // Quantidade no sistema
    public decimal PhysicalQuantity { get; set; }; // Quantidade contada
    public decimal Difference { get; set; };      // Diferença
    public Guid AdjustedBy { get; set; };        // Usuário
    public string AdjustmentReason { get; set; } = string.Empty;

    // Relacionamentos
    public virtual Product? Product { get; set; }
}
