namespace ERP.API.Controllers;

using ERP.Application.DTOs.Price;
using ERP.Application.Services.Prices;
using ERP.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller para cálculo de preços (Motor de Preços - CORE)
/// </summary>
[ApiController]
[Route("api/[controller}")]
[Authorize]
public class PricesController : ControllerBase
{
    private readonly IPriceCalculationService _priceCalculationService;
    private readonly ILogger<PricesController> _logger;

    public PricesController(IPriceCalculationService priceCalculationService, ILogger<PricesController> logger)
    {
        _priceCalculationService = priceCalculationService;
        _logger = logger;
    }

    /// <summary>
    /// Calcular preço de um produto (Motor de Preços)
    /// Aplica automaticamente regras e contexto
    /// </summary>
    [HttpPost("calculate")]
    [Authorize(Policy = "PricesView")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PriceCalculationResultDto>> CalculatePrice([FromBody] CalculatePriceDto dto)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        if (string.IsNullOrEmpty(companyId))
            return Unauthorized();

        _logger.LogInformation(
            "Calculando preço: Produto={ProductId}, Quantidade={Quantity}, Cliente={CustomerId}",
            dto.ProductId, dto.Quantity, dto.CustomerId);

        var result = await _priceCalculationService.CalculatePriceAsync(dto, Guid.Parse(companyId));
        return Ok(result);
    }
}
