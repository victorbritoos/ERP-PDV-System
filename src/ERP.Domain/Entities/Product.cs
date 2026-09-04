namespace ERP.Domain.Entities;

using ERP.Domain.Common;
using ERP.Domain.Enums;

/// <summary>
/// Entidade de produto (CORE DO SISTEMA)
/// </summary>
public class Product : BaseEntity
{
    public string Sku { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? BrandId { get; set; }
    public Guid? SupplierId { get; set; }
    public UnitType Unit { get; set; } = UnitType.Un;
    public decimal CostPrice { get; set; }
    public decimal StockMinimum { get; set; }
    public decimal StockMaximum { get; set; }
    public decimal CurrentStock { get; set; }
    public string? ImageUrl { get; set; }

    // Relacionamentos
    public virtual Category? Category { get; set; }
    public virtual Brand? Brand { get; set; }
    public virtual Supplier? Supplier { get; set; }
    public virtual ICollection<ProductPrice> Prices { get; set; } = new List<ProductPrice>();
    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}
