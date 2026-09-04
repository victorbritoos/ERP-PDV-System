namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Entidade de cliente
/// </summary>
public class Customer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Document { get; set; } // CPF/CNPJ
    public string? Phone { get; set; }
    public string? WhatsApp { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public decimal CreditLimit { get; set; } = 0;
    public Guid? DefaultPriceTableId { get; set; }
    public string? Notes { get; set; }

    // Relacionamentos
    public virtual PriceTable? DefaultPriceTable { get; set; }
    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    public virtual ICollection<CustomerPriceTable> PriceTables { get; set; } = new List<CustomerPriceTable>();
    public virtual ICollection<AccountsReceivable> AccountsReceivable { get; set; } = new List<AccountsReceivable>();
}
