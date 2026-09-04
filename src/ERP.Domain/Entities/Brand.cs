namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Entidade de marca de produtos
/// </summary>
public class Brand : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }

    // Relacionamentos
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
