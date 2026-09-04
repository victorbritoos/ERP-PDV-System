namespace ERP.Application.DTOs.PriceTable;

/// <summary>
/// DTO para criar uma nova tabela de preço
/// </summary>
public class CreatePriceTableDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Priority { get; set; } = 0;
    public bool IsDefault { get; set; } = false;
}

/// <summary>
/// DTO para atualizar uma tabela de preço
/// </summary>
public class UpdatePriceTableDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Priority { get; set; }
    public bool IsDefault { get; set; }
}

/// <summary>
/// DTO para retornar informações de uma tabela de preço
/// </summary>
public class PriceTableDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Priority { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int ProductCount { get; set; }
}

/// <summary>
/// DTO para listar tabelas de preço
/// </summary>
public class PriceTableListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}
