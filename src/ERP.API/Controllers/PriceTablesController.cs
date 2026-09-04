namespace ERP.API.Controllers;

using ERP.Application.DTOs.PriceTable;
using ERP.Application.Services.PriceTables;
using ERP.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

/// <summary>
/// Controller para gerenciamento de tabelas de preço
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PriceTablesController : ControllerBase
{
    private readonly IPriceTableService _priceTableService;
    private readonly ILogger<PriceTablesController> _logger;

    public PriceTablesController(IPriceTableService priceTableService, ILogger<PriceTablesController> logger)
    {
        _priceTableService = priceTableService;
        _logger = logger;
    }

    /// <summary>
    /// Listar todas as tabelas de preço ativas
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "PricesView")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<PriceTableListDto>>> GetAll()
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        if (string.IsNullOrEmpty(companyId))
            return Unauthorized();

        var tables = await _priceTableService.GetAllAsync(Guid.Parse(companyId));
        return Ok(tables);
    }

    /// <summary>
    /// Obter tabela de preço por ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = "PricesView")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PriceTableDto>> GetById(Guid id)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        if (string.IsNullOrEmpty(companyId))
            return Unauthorized();

        var table = await _priceTableService.GetByIdAsync(id, Guid.Parse(companyId));
        return Ok(table);
    }

    /// <summary>
    /// Criar nova tabela de preço
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "PriceTablesManage")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PriceTableDto>> Create([FromBody] CreatePriceTableDto dto)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(companyId) || string.IsNullOrEmpty(userId))
            return Unauthorized();

        var table = await _priceTableService.CreateAsync(dto, Guid.Parse(companyId), Guid.Parse(userId));
        return CreatedAtAction(nameof(GetById), new { id = table.Id }, table);
    }

    /// <summary>
    /// Atualizar tabela de preço
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "PriceTablesManage")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePriceTableDto dto)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(companyId) || string.IsNullOrEmpty(userId))
            return Unauthorized();

        await _priceTableService.UpdateAsync(id, dto, Guid.Parse(companyId), Guid.Parse(userId));
        return NoContent();
    }

    /// <summary>
    /// Deletar (desativar) tabela de preço
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "PriceTablesManage")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(companyId) || string.IsNullOrEmpty(userId))
            return Unauthorized();

        await _priceTableService.DeleteAsync(id, Guid.Parse(companyId), Guid.Parse(userId));
        return NoContent();
    }
}
