namespace ERP.Domain.Entities;

using ERP.Domain.Common;
using ERP.Domain.Enums;

/// <summary>
/// Despesas gerais da empresa
/// </summary>
public class Expense : BaseEntity
{
    public ExpenseCategory Category { get; set; };
    public decimal Amount { get; set; };
    public DateTime DueDate { get; set; };
    public DateTime? PaidAt { get; set; };
    public string? Supplier { get; set; };
    public string? Notes { get; set; };
    public bool IsPaid { get; set; } = false;

    // Sem relacionamentos específicos (pode ser expandido depois)
}
