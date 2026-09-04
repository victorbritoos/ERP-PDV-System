namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Entidade de usuário do sistema
/// </summary>
public class User : BaseEntity
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public bool EmailConfirmed { get; set; }
    public string? PasswordHash { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime? LockedOutUntil { get; set; }
    public int AccessFailedCount { get; set; }
    public bool LockoutEnabled { get; set; } = true;

    // Relacionamentos
    public virtual Company? Company { get; set; }
}
