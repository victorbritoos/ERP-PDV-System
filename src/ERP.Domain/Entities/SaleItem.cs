namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Itens de uma venda (PREÇO CONGELADO)
/// </summary>
public class SaleItem : BaseEntity
{
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public Guid PriceTableId { get; set; };         // Tabela de preço usada (congelada)
    public decimal UnitPrice { get; set; };        // Preço unitário (CONGELADO)
    public decimal Quantity { get; set; };
    public decimal Discount { get; set; } = 0;     // Desconto do item
    public decimal Total { get; set; };            // Total do item
    public string? PriceCalculationExplanation { get; set; }; // JSON com explicação

    // Relacionamentos
    public virtual Sale? Sale { get; set; }
    public virtual Product? Product { get; set; }
    public virtual PriceTable? PriceTable { get; set; }
}
