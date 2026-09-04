namespace ERP.API.Controllers;

using ERP.Application.DTOs.Sale;
using ERP.Application.Services.Prices;
using ERP.Application.Services.Products;
using ERP.Infrastructure.Data.Repositories;
using ERP.Domain.Entities;
using ERP.Domain.Exceptions;
using ERP.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

/// <summary>
/// Controller para operações de PDV (Ponto de Venda)
/// </summary>
[ApiController]
[Route("api/[controller}")]
[Authorize]
public class PointOfSaleController : ControllerBase
{
    private readonly IPriceCalculationService _priceCalculationService;
    private readonly IProductRepository _productRepository;
    private readonly IGenericRepository<Sale> _saleRepository;
    private readonly IGenericRepository<SaleItem> _saleItemRepository;
    private readonly IGenericRepository<StockMovement> _stockMovementRepository;
    private readonly ILogger<PointOfSaleController> _logger;

    public PointOfSaleController(
        IPriceCalculationService priceCalculationService,
        IProductRepository productRepository,
        IGenericRepository<Sale> saleRepository,
        IGenericRepository<SaleItem> saleItemRepository,
        IGenericRepository<StockMovement> stockMovementRepository,
        ILogger<PointOfSaleController> logger)
    {
        _priceCalculationService = priceCalculationService;
        _productRepository = productRepository;
        _saleRepository = saleRepository;
        _saleItemRepository = saleItemRepository;
        _stockMovementRepository = stockMovementRepository;
        _logger = logger;
    }

    /// <summary>
    /// Criar nova venda com itens e pagamentos
    /// </summary>
    [HttpPost("sales")]
    [Authorize(Policy = "PosSell")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SaleDto>> CreateSale([FromBody] CreateSaleDto dto)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(companyId) || string.IsNullOrEmpty(userId))
            return Unauthorized();

        var companyGuid = Guid.Parse(companyId);
        var userGuid = Guid.Parse(userId);

        try
        {
            // Validar itens
            if (!dto.Items.Any())
                throw new BusinessException("Venda deve conter pelo menos um item", "EMPTY_SALE");

            // Validar pagamentos
            if (!dto.Payments.Any())
                throw new BusinessException("Venda deve conter pelo menos um pagamento", "NO_PAYMENTS");

            // Calcular total
            decimal saleTotal = 0;
            var saleItems = new List<SaleItem>();

            foreach (var itemDto in dto.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null || product.CompanyId != companyGuid)
                    throw new NotFoundException($"Produto não encontrado");

                // Calcular preço
                var priceResult = await _priceCalculationService.CalculatePriceAsync(
                    new Application.DTOs.Price.CalculatePriceDto
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity,
                        CustomerId = dto.CustomerId,
                        ForcedPriceTableId = itemDto.ForcedPrice.HasValue ? dto.PriceTableId : null
                    },
                    companyGuid);

                // Usar preço forçado se fornecido
                var unitPrice = itemDto.ForcedPrice ?? priceResult.UnitPrice;
                var itemTotal = unitPrice * itemDto.Quantity;

                saleItems.Add(new SaleItem
                {
                    Id = Guid.NewGuid(),
                    CompanyId = companyGuid,
                    ProductId = itemDto.ProductId,
                    PriceTableId = dto.PriceTableId,
                    UnitPrice = unitPrice,
                    Quantity = itemDto.Quantity,
                    Discount = 0,
                    Total = itemTotal,
                    PriceCalculationExplanation = priceResult.Explanation,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = userGuid
                });

                saleTotal += itemTotal;
                
                // Validar estoque
                if (product.CurrentStock < itemDto.Quantity)
                    throw new BusinessException(
                        $"Estoque insuficiente para {product.Name}. Disponível: {product.CurrentStock}",
                        "INSUFFICIENT_STOCK");
            }

            // Aplicar desconto total
            saleTotal -= dto.Discount;
            if (saleTotal < 0)
                saleTotal = 0;

            // Validar total de pagamentos
            var paymentTotal = dto.Payments.Sum(p => p.Amount);
            if (paymentTotal < saleTotal)
                throw new BusinessException(
                    $"Total de pagamentos ({paymentTotal}) menor que total da venda ({saleTotal})",
                    "INSUFFICIENT_PAYMENT");

            // Criar venda
            var saleNumber = GenerateSalesNumber(companyGuid);
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                CompanyId = companyGuid,
                SalesNumber = saleNumber,
                CustomerId = dto.CustomerId,
                PriceTableId = dto.PriceTableId,
                SalesUserId = userGuid,
                CashRegisterId = dto.CashRegisterId,
                Discount = dto.Discount,
                DiscountReason = dto.DiscountReason,
                Total = saleTotal,
                Status = (int)ERP.Domain.Enums.SaleStatus.Completed,
                SalesAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userGuid,
                Items = saleItems
            };

            await _saleRepository.AddAsync(sale);

            // Atualizar estoque
            foreach (var item in saleItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.CurrentStock -= item.Quantity;
                    await _productRepository.UpdateAsync(product);

                    // Registrar movimento de estoque
                    var movement = new StockMovement
                    {
                        Id = Guid.NewGuid(),
                        CompanyId = companyGuid,
                        ProductId = item.ProductId,
                        MovementType = (int)ERP.Domain.Enums.StockMovementType.Sale,
                        Quantity = -item.Quantity,
                        ReferencedSaleId = sale.Id,
                        RecordedBy = userGuid,
                        RecordedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _stockMovementRepository.AddAsync(movement);
                }
            }

            _logger.LogInformation(
                "Venda criada: {SalesNumber}, Total: {Total}, Itens: {ItemCount}",
                saleNumber, saleTotal, saleItems.Count);

            return CreatedAtAction(nameof(GetSale), new { id = sale.Id }, sale);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar venda");
            throw;
        }
    }

    /// <summary>
    /// Obter dados de uma venda
    /// </summary>
    [HttpGet("sales/{id}")]
    [Authorize(Policy = "PosSell")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Sale>> GetSale(Guid id)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        if (string.IsNullOrEmpty(companyId))
            return Unauthorized();

        var sale = await _saleRepository.GetByIdAsync(id);
        if (sale == null || sale.CompanyId != Guid.Parse(companyId))
            return NotFound();

        return Ok(sale);
    }

    /// <summary>
    /// Cancelar venda
    /// </summary>
    [HttpPost("sales/{id}/cancel")]
    [Authorize(Policy = "PosCancelSale")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelSale(Guid id, [FromBody] CancelSaleRequest request)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(companyId) || string.IsNullOrEmpty(userId))
            return Unauthorized();

        var companyGuid = Guid.Parse(companyId);
        var userGuid = Guid.Parse(userId);

        var sale = await _saleRepository.GetByIdAsync(id);
        if (sale == null || sale.CompanyId != companyGuid)
            return NotFound();

        if (sale.Status == (int)ERP.Domain.Enums.SaleStatus.Cancelled)
            return BadRequest("Venda já foi cancelada");

        // Revertir estoque
        foreach (var item in sale.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product != null)
            {
                product.CurrentStock += item.Quantity;
                await _productRepository.UpdateAsync(product);

                // Registrar reversão
                var movement = new StockMovement
                {
                    Id = Guid.NewGuid(),
                    CompanyId = companyGuid,
                    ProductId = item.ProductId,
                    MovementType = (int)ERP.Domain.Enums.StockMovementType.SaleReturn,
                    Quantity = item.Quantity,
                    ReferencedSaleId = sale.Id,
                    RecordedBy = userGuid,
                    RecordedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _stockMovementRepository.AddAsync(movement);
            }
        }

        // Cancelar venda
        sale.Status = (int)ERP.Domain.Enums.SaleStatus.Cancelled;
        sale.UpdatedAt = DateTime.UtcNow;
        sale.UpdatedBy = userGuid;
        await _saleRepository.UpdateAsync(sale);

        // Criar registro de cancelamento
        var cancellation = new SaleCancellation
        {
            Id = Guid.NewGuid(),
            CompanyId = companyGuid,
            SaleId = id,
            CancellationReason = request.Reason,
            CancelledBy = userGuid,
            CancelledAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _saleRepository.AddAsync(cancellation);

        _logger.LogInformation("Venda {SalesNumber} cancelada por {User}", sale.SalesNumber, userGuid);

        return Ok("Venda cancelada com sucesso");
    }

    #region Private Methods

    private string GenerateSalesNumber(Guid companyId)
    {
        // Formato: PDV-AAAAMMDD-NNNNN
        var date = DateTime.Now.ToString("yyyyMMdd");
        var random = new Random().Next(10000, 99999);
        return $"PDV-{date}-{random}";
    }

    #endregion
}

public class CancelSaleRequest
{
    public string Reason { get; set; } = string.Empty;
}
