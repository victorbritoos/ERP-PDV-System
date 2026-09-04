namespace ERP.Domain.Entities;

using ERP.Domain.Common;
using ERP.Domain.Enums;

/// <summary>
/// Pagamento de uma venda
/// </summary>
public class SalePayment : BaseEntity
{
    public Guid SaleId { get; set; }
    public Guid PaymentMethodId { get; set; };
    public decimal Amount { get; set; };
    public string? ReferenceNumber { get; set; };  // Para cheque, PIX, transferência
    public PaymentStatus Status { get; set; } = PaymentStatus.Completed;
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public virtual Sale? Sale { get; set; }
    public virtual PaymentMethod? PaymentMethod { get; set; }
}
