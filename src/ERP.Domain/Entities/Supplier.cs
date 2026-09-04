namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Entidade de fornecedor
/// </summary>
public class Supplier : BaseEntity
{
    public string LegalName { get; set; } = string.Empty;
    public string? TradingName { get; set; }
    public string? Document { get; set; } // CPF/CNPJ
    public string? Phone { get; set; }
    public string? WhatsApp { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Notes { get; set; }

    // Relacionamentos
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
