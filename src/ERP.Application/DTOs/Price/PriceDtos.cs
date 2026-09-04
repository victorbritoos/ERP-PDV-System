namespace ERP.Application.DTOs.Price;

/// <summary>
/// DTO para calcular preço de um produto
/// </summary>
public class CalculatePriceDto
{
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public int PaymentMethodType { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? ForcedPriceTableId { get; set; }
}

/// <summary>
/// DTO para resultado do cálculo de preço
/// </summary>
public class PriceCalculationResultDto
{
    public Guid ProductId { get; set; }
    public Guid PriceTableId { get; set; }
    public string? PriceTableName { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }
    public decimal Total { get; set; }
    public string? AppliedRuleName { get; set; }
    public string? Explanation { get; set; }
    public DateTime CalculatedAt { get; set; }
}

/// <summary>
/// DTO para alterar preço de um produto em uma tabela
/// </summary>
public class UpdateProductPriceDto
{
    public Guid ProductId { get; set; }
    public Guid PriceTableId { get; set; }
    public decimal NewPrice { get; set; }
    public string? ChangeReason { get; set; }
}

/// <summary>
/// DTO para alteração em massa de preços
/// </summary>
public class BulkPriceUpdateDto
{
    public List<Guid>? ProductIds { get; set; }
    public List<Guid>? CategoryIds { get; set; }
    public List<Guid>? SupplierIds { get; set; }
    public Guid PriceTableId { get; set; }
    public decimal? PercentageIncrease { get; set; }
    public decimal? PercentageDecrease { get; set; }
    public decimal? ValueIncrease { get; set; }
    public decimal? ValueDecrease { get; set; }
    public string? Reason { get; set; }
}
