namespace ERP.Domain.Entities;

using ERP.Domain.Common;
using ERP.Domain.Enums;

/// <summary>
/// Log de auditoria - registra todas as operações críticas
/// </summary>
public class AuditLog : BaseEntity
{
    public Guid? UserId { get; set; };
    public string EntityName { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public AuditActionType Action { get; set; };
    public string? OldValues { get; set; };     // JSON
    public string? NewValues { get; set; };     // JSON
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? IpAddress { get; set; };
    public string? UserAgent { get; set; };
    public string? Description { get; set; };
}
