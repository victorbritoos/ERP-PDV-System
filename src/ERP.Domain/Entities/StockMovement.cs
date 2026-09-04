namespace ERP.Domain.Entities;

using ERP.Domain.Common;
using ERP.Domain.Enums;

/// <summary>
/// Movimentação de estoque
/// </summary>
public class StockMovement : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid? WarehouseId { get; set; };        // Para futuro suporte a múltiplos depósitos
    public StockMovementType MovementType { get; set; };
    public decimal Quantity { get; set; };        // Pode ser negativo
    public Guid? ReferencedSaleId { get; set; };  // Se for venda
    public Guid? ReferencedPurchaseOrderId { get; set; }; // Se for compra
    public string? Notes { get; set; };
    public Guid RecordedBy { get; set; };
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public virtual Product? Product { get; set; }
}
