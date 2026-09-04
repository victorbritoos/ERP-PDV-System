namespace ERP.Application.DTOs.Product;

/// <summary>
/// DTO para criar um novo produto
/// </summary>
public class CreateProductDto
{
    public string Sku { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? BrandId { get; set; }
    public Guid? SupplierId { get; set; }
    public int Unit { get; set; } = 1; // UnitType.Un
    public decimal CostPrice { get; set; }
    public decimal StockMinimum { get; set; }
    public decimal StockMaximum { get; set; }
    public string? ImageUrl { get; set; }
}

/// <summary>
/// DTO para atualizar um produto
/// </summary>
public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? BrandId { get; set; }
    public Guid? SupplierId { get; set; }
    public decimal CostPrice { get; set; }
    public decimal StockMinimum { get; set; }
    public decimal StockMaximum { get; set; }
    public string? ImageUrl { get; set; }
}

/// <summary>
/// DTO para retornar informações de um produto
/// </summary>
public class ProductDto
{
    public Guid Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public Guid? BrandId { get; set; }
    public string? BrandName { get; set; }
    public Guid? SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public int Unit { get; set; }
    public decimal CostPrice { get; set; }
    public decimal StockMinimum { get; set; }
    public decimal StockMaximum { get; set; }
    public decimal CurrentStock { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<ProductPriceDto> Prices { get; set; } = new List<ProductPriceDto>();
}

/// <summary>
/// DTO simplificado de produto (para listas)
/// </summary>
public class ProductListDto
{
    public Guid Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? CategoryName { get; set; }
    public decimal CostPrice { get; set; }
    public decimal CurrentStock { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO para preço de produto
/// </summary>
public class ProductPriceDto
{
    public Guid Id { get; set; }
    public Guid PriceTableId { get; set; }
    public string? PriceTableName { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
}
