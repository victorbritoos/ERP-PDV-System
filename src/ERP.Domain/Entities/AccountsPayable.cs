namespace ERP.Domain.Entities;

using ERP.Domain.Common;
using ERP.Domain.Enums;

/// <summary>
/// Contas a pagar
/// </summary>
public class AccountsPayable : BaseEntity
{
    public Guid SupplierId { get; set; };
    public Guid? PurchaseOrderId { get; set; };  // Referência de compra
    public decimal Amount { get; set; };
    public DateTime DueDate { get; set; };
    public AccountStatus Status { get; set; } = AccountStatus.Pending;
    public bool IsRecurring { get; set; } = false;
    public RecurrenceInterval? RecurrenceInterval { get; set; };
    public string? Notes { get; set; };

    // Relacionamentos
    public virtual Supplier? Supplier { get; set; }
    public virtual ICollection<AccountsPayablePayment> Payments { get; set; } = new List<AccountsPayablePayment>();
}

/// <summary>
/// Pagamento de contas a pagar
/// </summary>
public class AccountsPayablePayment : BaseEntity
{
    public Guid AccountsPayableId { get; set; }
    public decimal Amount { get; set; };
    public Guid PaymentMethodId { get; set; };
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    public Guid PaidBy { get; set; };           // Usuário

    // Relacionamentos
    public virtual AccountsPayable? AccountsPayable { get; set; }
    public virtual PaymentMethod? PaymentMethod { get; set; }
}
