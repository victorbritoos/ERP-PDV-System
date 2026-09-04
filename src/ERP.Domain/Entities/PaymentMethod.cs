namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Forma de pagamento
/// </summary>
public class PaymentMethod : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    // Relacionamentos
    public virtual ICollection<SalePayment> SalePayments { get; set; } = new List<SalePayment>();
}
