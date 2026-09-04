namespace ERP.Domain.Entities;

using ERP.Domain.Common;
using ERP.Domain.Enums;

/// <summary>
/// Sessão de caixa (abertura e fechamento)
/// </summary>
public class CashRegister : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    // Relacionamentos
    public virtual ICollection<CashRegisterSession> Sessions { get; set; } = new List<CashRegisterSession>();
    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}

/// <summary>
/// Sessão de operação do caixa
/// </summary>
public class CashRegisterSession : BaseEntity
{
    public Guid CashRegisterId { get; set; }
    public Guid OpenedBy { get; set; };          // Usuário que abriu
    public DateTime OpenedAt { get; set; } = DateTime.UtcNow;
    public Guid? ClosedBy { get; set; };         // Usuário que fechou
    public DateTime? ClosedAt { get; set; };
    public decimal InitialBalance { get; set; } = 0;  // Saldo inicial
    public decimal? FinalBalance { get; set; };      // Saldo final informado
    public decimal? BalanceDifference { get; set; }; // Diferença
    public string? Notes { get; set; };
    public CashSessionStatus Status { get; set; } = CashSessionStatus.Open;

    // Relacionamentos
    public virtual CashRegister? CashRegister { get; set; }
    public virtual ICollection<CashRegisterMovement> Movements { get; set; } = new List<CashRegisterMovement>();
}

/// <summary>
/// Movimentação de caixa
/// </summary>
public class CashRegisterMovement : BaseEntity
{
    public Guid CashRegisterSessionId { get; set; }
    public CashMovementType MovementType { get; set; };
    public decimal Amount { get; set; };
    public string Description { get; set; } = string.Empty;
    public Guid? ReferencedSaleId { get; set; };  // Se for venda
    public Guid RecordedBy { get; set; };        // Usuário
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public virtual CashRegisterSession? CashRegisterSession { get; set; }
}
