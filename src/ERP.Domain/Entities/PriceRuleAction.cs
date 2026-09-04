namespace ERP.Domain.Entities;

using ERP.Domain.Common;

/// <summary>
/// Ação de uma regra de preço
/// </summary>
public class PriceRuleAction : BaseEntity
{
    public Guid PriceRuleId { get; set; }
    public string ActionType { get; set; } = string.Empty;    // USE_PRICE_TABLE, etc
    public Guid? TargetPriceTableId { get; set; };            // Tabela de preço alvo

    // Relacionamentos
    public virtual PriceRule? PriceRule { get; set; }
    public virtual PriceTable? TargetPriceTable { get; set; }
}
