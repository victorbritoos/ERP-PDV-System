namespace ERP.Application.DTOs.Sale;

using ERP.Application.DTOs.Product;

/// <summary>
/// DTO para adicionar um item ao carrinho de vendas
/// </summary>
public class AddSaleItemDto
{
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal? ForcedPrice { get; set; } // Se quiser forçar um preço manualmente
}

/// <summary>
/// DTO para item de venda
/// </summary>
public class SaleItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSku { get; set; } = string.Empty;
    public Guid PriceTableId { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
    public string? PriceCalculationExplanation { get; set; }
}

/// <summary>
/// DTO para criar uma nova venda
/// </summary>
public class CreateSaleDto
{
    public Guid? CustomerId { get; set; }
    public Guid PriceTableId { get; set; }
    public Guid CashRegisterId { get; set; }
    public decimal Discount { get; set; } = 0;
    public string? DiscountReason { get; set; }
    public List<AddSaleItemDto> Items { get; set; } = new();
    public List<CreateSalePaymentDto> Payments { get; set; } = new();
    public string? Notes { get; set; }
}

/// <summary>
/// DTO para pagamento de venda
/// </summary>
public class CreateSalePaymentDto
{
    public Guid PaymentMethodId { get; set; }
    public decimal Amount { get; set; }
    public string? ReferenceNumber { get; set; }
}

/// <summary>
/// DTO para retornar informações de uma venda
/// </summary>
public class SaleDto
{
    public Guid Id { get; set; }
    public string SalesNumber { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public Guid PriceTableId { get; set; }
    public string? PriceTableName { get; set; }
    public decimal Total { get; set; }
    public decimal Discount { get; set; }
    public int Status { get; set; }
    public DateTime SalesAt { get; set; }
    public List<SaleItemDto> Items { get; set; } = new();
    public List<SalePaymentDto> Payments { get; set; } = new();
}

/// <summary>
/// DTO para pagamento de venda (retorno)
/// </summary>
public class SalePaymentDto
{
    public Guid Id { get; set; }
    public Guid PaymentMethodId { get; set; }
    public string? PaymentMethodName { get; set; }
    public decimal Amount { get; set; }
    public int Status { get; set; }
}
