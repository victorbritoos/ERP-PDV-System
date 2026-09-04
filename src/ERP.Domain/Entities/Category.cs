namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Entidade de categoria de produtos
/// </summary>
public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? ParentCategoryId { get; set; } // Para subcategorias
    public int DisplayOrder { get; set; }
    public string? ImageUrl { get; set; }

    // Relacionamentos
    public virtual Category? ParentCategory { get; set; }
    public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
