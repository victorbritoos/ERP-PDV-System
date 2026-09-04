namespace ERP.Domain.Common;

/// <summary>
/// Classe base para todas as entidades do domínio
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identificador único da entidade
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identificador da empresa (multiempresa)
    /// </summary>
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Indica se a entidade está ativa
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Data e hora de criação
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Data e hora da última atualização
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// ID do usuário que criou o registro
    /// </summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// ID do usuário que atualizou o registro
    /// </summary>
    public Guid? UpdatedBy { get; set; }
}
