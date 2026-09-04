namespace ERP.Domain.Entities;

using ERP.Domain.Common;
using ERP.Domain.Enums;

/// <summary>
/// Entidade de venda (PDV)
/// </summary>
public class Sale : BaseEntity
{
    public string SalesNumber { get; set; } = string.Empty;  // Sequencial único
    public Guid? CustomerId { get; set; };
    public Guid PriceTableId { get; set; };                 // Tabela utilizada na venda
    public Guid SalesUserId { get; set; };                  // Operador/Vendedor
    public Guid? CashRegisterId { get; set; };
    public decimal Discount { get; set; } = 0;             // Desconto total
    public string? DiscountReason { get; set; };
    public decimal Total { get; set; } = 0;                // Total final
    public string? Notes { get; set; };
    public SaleStatus Status { get; set; } = SaleStatus.Open;
    public DateTime SalesAt { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public virtual Customer? Customer { get; set; }
    public virtual PriceTable? PriceTable { get; set; }
    public virtual User? SalesUser { get; set; }
    public virtual CashRegister? CashRegister { get; set; }
    public virtual ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
    public virtual ICollection<SalePayment> Payments { get; set; } = new List<SalePayment>();
    public virtual SaleCancellation? Cancellation { get; set; }
}
