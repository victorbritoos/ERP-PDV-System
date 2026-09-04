namespace ERP.API.Controllers;

using ERP.Application.DTOs.Customer;
using ERP.Application.Services.Customers;
using ERP.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

/// <summary>
/// Controller para gerenciamento de clientes
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    /// <summary>
    /// Listar todos os clientes com paginação
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "CustomersView")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<CustomerListDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        if (string.IsNullOrEmpty(companyId))
            return Unauthorized();

        var customers = await _customerService.GetAllAsync(Guid.Parse(companyId), page, pageSize);
        return Ok(customers);
    }

    /// <summary>
    /// Obter cliente por ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = "CustomersView")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        if (string.IsNullOrEmpty(companyId))
            return Unauthorized();

        var customer = await _customerService.GetByIdAsync(id, Guid.Parse(companyId));
        return Ok(customer);
    }

    /// <summary>
    /// Buscar clientes por termo
    /// </summary>
    [HttpGet("search/{searchTerm}")]
    [Authorize(Policy = "CustomersView")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CustomerListDto>>> Search(
        string searchTerm,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        if (string.IsNullOrEmpty(companyId))
            return Unauthorized();

        var customers = await _customerService.SearchAsync(searchTerm, Guid.Parse(companyId), page, pageSize);
        return Ok(customers);
    }

    /// <summary>
    /// Criar novo cliente
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "CustomersCreate")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerDto dto)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(companyId) || string.IsNullOrEmpty(userId))
            return Unauthorized();

        var customer = await _customerService.CreateAsync(dto, Guid.Parse(companyId), Guid.Parse(userId));
        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    /// <summary>
    /// Atualizar cliente
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "CustomersEdit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerDto dto)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(companyId) || string.IsNullOrEmpty(userId))
            return Unauthorized();

        await _customerService.UpdateAsync(id, dto, Guid.Parse(companyId), Guid.Parse(userId));
        return NoContent();
    }

    /// <summary>
    /// Deletar (desativar) cliente
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "CustomersEdit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var companyId = User.FindFirst("CompanyId")?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(companyId) || string.IsNullOrEmpty(userId))
            return Unauthorized();

        await _customerService.DeleteAsync(id, Guid.Parse(companyId), Guid.Parse(userId));
        return NoContent();
    }
}
