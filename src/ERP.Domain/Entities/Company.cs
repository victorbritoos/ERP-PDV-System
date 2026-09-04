namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Entidade de empresa (suporta multiempresa)
/// </summary>
public class Company : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? TradingName { get; set; }
    public string? Document { get; set; } // CNPJ
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Phone { get; set; }
    public string? WhatsApp { get; set; }
    public string? Email { get; set; }
    public string? LogoUrl { get; set; }
    public string? Notes { get; set; }

    // Relacionamentos
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    public virtual ICollection<Brand> Brands { get; set; } = new List<Brand>();
    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
    public virtual ICollection<PriceTable> PriceTables { get; set; } = new List<PriceTable>();
    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    public virtual ICollection<CashRegister> CashRegisters { get; set; } = new List<CashRegister>();
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
