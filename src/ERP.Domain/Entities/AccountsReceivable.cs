namespace ERP.Domain.Entities;

using ERP.Domain.Common;
using ERP.Domain.Enums;

/// <summary>
/// Contas a receber
/// </summary>
public class AccountsReceivable : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Guid? SaleId { get; set; };           // Referência de venda
    public decimal Amount { get; set; };
    public DateTime DueDate { get; set; };
    public AccountStatus Status { get; set; } = AccountStatus.Pending;
    public string? Notes { get; set; };

    // Relacionamentos
    public virtual Customer? Customer { get; set; }
    public virtual Sale? Sale { get; set; }
    public virtual ICollection<AccountsReceivablePayment> Payments { get; set; } = new List<AccountsReceivablePayment>();
}

/// <summary>
/// Pagamento de contas a receber
/// </summary>
public class AccountsReceivablePayment : BaseEntity
{
    public Guid AccountsReceivableId { get; set; }
    public decimal Amount { get; set; };
    public Guid PaymentMethodId { get; set; };
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    public Guid ReceivedBy { get; set; };       // Usuário

    // Relacionamentos
    public virtual AccountsReceivable? AccountsReceivable { get; set; }
    public virtual PaymentMethod? PaymentMethod { get; set; }
}
