namespace ERP.Application.Services.Prices;

using ERP.Application.DTOs.Price;
using ERP.Domain.Entities;
using ERP.Domain.Exceptions;
using ERP.Infrastructure.Data.Repositories;
using System.Text.Json;

/// <summary>
/// Serviço central para cálculo de preços (CORE DO SISTEMA)
/// </summary>
public interface IPriceCalculationService
{
    Task<PriceCalculationResultDto> CalculatePriceAsync(CalculatePriceDto request, Guid companyId);
    Task<decimal> GetProductPriceAsync(Guid productId, Guid priceTableId, Guid companyId);
}

public class PriceCalculationService : IPriceCalculationService
{
    private readonly IProductPriceRepository _productPriceRepository;
    private readonly IPriceTableRepository _priceTableRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;

    public PriceCalculationService(
        IProductPriceRepository productPriceRepository,
        IPriceTableRepository priceTableRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository)
    {
        _productPriceRepository = productPriceRepository;
        _priceTableRepository = priceTableRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// Calcula o preço de um produto considerando regras e contexto
    /// </summary>
    public async Task<PriceCalculationResultDto> CalculatePriceAsync(CalculatePriceDto request, Guid companyId)
    {
        // Validar produto
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            throw new NotFoundException($"Produto com ID {request.ProductId} não encontrado");

        // Se foi forçado uma tabela de preço, usar essa
        Guid priceTableId = request.ForcedPriceTableId ?? Guid.Empty;
        string? appliedRuleName = null;

        if (priceTableId == Guid.Empty)
        {
            // Verificar se cliente tem tabela padrão
            if (request.CustomerId.HasValue && request.CustomerId.Value != Guid.Empty)
            {
                var customer = await _customerRepository.GetByIdAsync(request.CustomerId.Value);
                if (customer?.DefaultPriceTableId != null)
                {
                    priceTableId = customer.DefaultPriceTableId.Value;
                    appliedRuleName = "Tabela padrão do cliente";
                }
            }

            // Se ainda não tem tabela, usar a padrão do sistema
            if (priceTableId == Guid.Empty)
            {
                var defaultTable = await _priceTableRepository.GetDefaultAsync(companyId);
                if (defaultTable == null)
                    throw new BusinessException("Nenhuma tabela de preço padrão configurada");

                priceTableId = defaultTable.Id;
                appliedRuleName = "Tabela padrão do sistema";
            }
        }

        // Obter preço do produto
        var productPrice = await _productPriceRepository.GetCurrentPriceAsync(request.ProductId, priceTableId);
        if (productPrice == null)
            throw new BusinessException($"Preço não encontrado para este produto nesta tabela");

        var priceTable = await _priceTableRepository.GetByIdAsync(priceTableId);
        var unitPrice = productPrice.UnitPrice;
        var total = unitPrice * request.Quantity;

        // Criar explicação
        var explanation = new
        {
            productId = request.ProductId,
            quantity = request.Quantity,
            appliedRule = appliedRuleName,
            priceTableId = priceTableId,
            priceTableName = priceTable?.Name,
            unitPrice = unitPrice,
            total = total,
            calculatedAt = DateTime.UtcNow
        };

        return new PriceCalculationResultDto
        {
            ProductId = request.ProductId,
            PriceTableId = priceTableId,
            PriceTableName = priceTable?.Name,
            UnitPrice = unitPrice,
            Quantity = request.Quantity,
            Total = total,
            AppliedRuleName = appliedRuleName,
            Explanation = JsonSerializer.Serialize(explanation),
            CalculatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Obter preço de um produto em uma tabela específica
    /// </summary>
    public async Task<decimal> GetProductPriceAsync(Guid productId, Guid priceTableId, Guid companyId)
    {
        var productPrice = await _productPriceRepository.GetCurrentPriceAsync(productId, priceTableId);
        if (productPrice == null)
            throw new NotFoundException($"Preço não encontrado para este produto");

        return productPrice.UnitPrice;
    }
}
