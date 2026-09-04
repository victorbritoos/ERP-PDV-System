namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Entidade de regra de preço (MOTOR DE REGRAS)
/// </summary>
public class PriceRule : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Priority { get; set; } = 0;  // Prioridade de execução

    // Relacionamentos
    public virtual ICollection<PriceRuleCondition> Conditions { get; set; } = new List<PriceRuleCondition>();
    public virtual ICollection<PriceRuleAction> Actions { get; set; } = new List<PriceRuleAction>();
}
