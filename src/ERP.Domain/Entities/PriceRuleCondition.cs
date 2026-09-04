namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Condição de uma regra de preço
/// </summary>
public class PriceRuleCondition : BaseEntity
{
    public Guid PriceRuleId { get; set; }
    public string ConditionType { get; set; } = string.Empty; // QUANTITY, PAYMENT_METHOD, CUSTOMER_TABLE, etc
    public string Operator { get; set; } = string.Empty;      // EQUAL, GREATER_THAN, LESS_THAN, etc
    public string Value { get; set; } = string.Empty;         // Valor da condição (pode ser complexo)

    // Relacionamentos
    public virtual PriceRule? PriceRule { get; set; }
}
