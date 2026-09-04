namespace ERP.API.Controllers;

using ERP.Application.DTOs.Product;
using ERP.Application.Services.Products;
using ERP.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

/// <summary>
/// Controller para gerenciamento de estoque
/// </summary>
[ApiController]
[Route("api/[controller}")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IGenericRepository<ERP.Domain.Entities.StockMovement> _stockMovementRepository;
    private readonly ILogger<InventoryController> _logger;

    public InventoryController(
        IProductRepository productRepository,
        IGenericRepository<ERP.Domain.Entities.StockMovement> stockMovementRepository,
        ILogger<InventoryController> logger)
    {
        _productRepository = productRepository;
        _stockMovementRepository = stockMovementRepository;
        _logger = logger;
    }

    /// <summary>
    /// Ajustar estoque de um produto
    /// </summary>
    [HttpPost("adjust")]
    [Authorize(Policy = "InventoryAdjust")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AdjustStock([FromBody] AdjustStockRequest request)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(companyId) || string.IsNullOrEmpty(userId))
            return Unauthorized();

        var companyGuid = Guid.Parse(companyId);
        var userGuid = Guid.Parse(userId);

        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null || product.CompanyId != companyGuid)
            return NotFound();

        var oldStock = product.CurrentStock;
        product.CurrentStock += request.Quantity;

        if (product.CurrentStock < 0)
            return BadRequest("Quantidade de estoque não pode ser negativa");

        await _productRepository.UpdateAsync(product);

        // Registrar movimento
        var movement = new ERP.Domain.Entities.StockMovement
        {
            Id = Guid.NewGuid(),
            CompanyId = companyGuid,
            ProductId = request.ProductId,
            MovementType = (int)ERP.Domain.Enums.StockMovementType.Adjustment,
            Quantity = request.Quantity,
            Notes = request.Reason,
            RecordedBy = userGuid,
            RecordedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = userGuid
        };
        await _stockMovementRepository.AddAsync(movement);

        _logger.LogInformation(
            "Ajuste de estoque: Produto={ProductId}, Anterior={OldStock}, Novo={NewStock}, Motivo={Reason}",
            request.ProductId, oldStock, product.CurrentStock, request.Reason);

        return Ok(new { productId = product.Id, oldStock = oldStock, newStock = product.CurrentStock });
    }

    /// <summary>
    /// Obter histórico de movimentações de estoque de um produto
    /// </summary>
    [HttpGet("movements/{productId}")]
    [Authorize(Policy = "InventoryView")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStockMovements(Guid productId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        if (string.IsNullOrEmpty(companyId))
            return Unauthorized();

        // Aqui seria implementada a busca de movimentos com paginação
        return Ok("Implementar retorno de movimentos de estoque");
    }
}

public class AdjustStockRequest
{
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
}
