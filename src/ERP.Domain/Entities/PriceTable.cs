namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Entidade de tabela de preço (MOTOR DE PREÇOS)
/// </summary>
public class PriceTable : BaseEntity
{
    public string Name { get; set; } = string.Empty;      // BASE, DINHEIRO, PIX, ATACADO, etc
    public string? Description { get; set; }
    public int Priority { get; set; } = 0;               // Prioridade (0 = padrão, 1 = mais alto)
    public bool IsDefault { get; set; } = false;         // É a tabela padrão?

    // Relacionamentos
    public virtual ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();
    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    public virtual ICollection<CustomerPriceTable> CustomerPriceTables { get; set; } = new List<CustomerPriceTable>();
}
