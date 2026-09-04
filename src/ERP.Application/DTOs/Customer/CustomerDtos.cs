namespace ERP.Application.DTOs.Customer;

/// <summary>
/// DTO para criar um novo cliente
/// </summary>
public class CreateCustomerDto
{
    public string Name { get; set; } = string.Empty;
    public string? Document { get; set; }
    public string? Phone { get; set; }
    public string? WhatsApp { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public decimal CreditLimit { get; set; } = 0;
    public Guid? DefaultPriceTableId { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// DTO para atualizar um cliente
/// </summary>
public class UpdateCustomerDto
{
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? WhatsApp { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public decimal CreditLimit { get; set; }
    public Guid? DefaultPriceTableId { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// DTO para retornar informações de um cliente
/// </summary>
public class CustomerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Document { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public decimal CreditLimit { get; set; }
    public Guid? DefaultPriceTableId { get; set; }
    public string? DefaultPriceTableName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO simplificado para listar clientes
/// </summary>
public class CustomerListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Document { get; set; }
    public string? Phone { get; set; }
    public decimal CreditLimit { get; set; }
    public bool IsActive { get; set; }
}
