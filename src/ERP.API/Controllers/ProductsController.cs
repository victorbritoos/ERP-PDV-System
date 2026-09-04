namespace ERP.API.Controllers;

using ERP.Application.DTOs.Product;
using ERP.Application.Services.Products;
using ERP.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

/// <summary>
/// Controller para gerenciamento de produtos
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Listar todos os produtos com paginação
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "ProductsView")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<ProductListDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var companyId = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyId))
                return Unauthorized("CompanyId not found in token");

            var products = await _productService.GetAllAsync(Guid.Parse(companyId), page, pageSize);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar produtos");
            throw;
        }
    }

    /// <summary>
    /// Obter produto por ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = "ProductsView")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetById(Guid id)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        if (string.IsNullOrEmpty(companyId))
            return Unauthorized();

        var product = await _productService.GetByIdAsync(id, Guid.Parse(companyId));
        return Ok(product);
    }

    /// <summary>
    /// Buscar produtos por termo
    /// </summary>
    [HttpGet("search/{searchTerm}")]
    [Authorize(Policy = "ProductsView")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductListDto>>> Search(
        string searchTerm,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        if (string.IsNullOrEmpty(companyId))
            return Unauthorized();

        var products = await _productService.SearchAsync(searchTerm, Guid.Parse(companyId), page, pageSize);
        return Ok(products);
    }

    /// <summary>
    /// Criar novo produto
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "ProductsCreate")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto dto)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(companyId) || string.IsNullOrEmpty(userId))
            return Unauthorized();

        var product = await _productService.CreateAsync(dto, Guid.Parse(companyId), Guid.Parse(userId));
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    /// <summary>
    /// Atualizar produto
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "ProductsEdit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(companyId) || string.IsNullOrEmpty(userId))
            return Unauthorized();

        await _productService.UpdateAsync(id, dto, Guid.Parse(companyId), Guid.Parse(userId));
        return NoContent();
    }

    /// <summary>
    /// Deletar (desativar) produto
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "ProductsDelete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(companyId) || string.IsNullOrEmpty(userId))
            return Unauthorized();

        await _productService.DeleteAsync(id, Guid.Parse(companyId), Guid.Parse(userId));
        return NoContent();
    }
}
