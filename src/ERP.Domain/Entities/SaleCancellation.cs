namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Cancelamento de venda
/// </summary>
public class SaleCancellation : BaseEntity
{
    public Guid SaleId { get; set; }
    public string CancellationReason { get; set; } = string.Empty;
    public Guid CancelledBy { get; set; };  // Usuário que cancelou
    public DateTime CancelledAt { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; };

    // Relacionamentos
    public virtual Sale? Sale { get; set; }
}
