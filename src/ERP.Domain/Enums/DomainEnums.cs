namespace ERP.Domain.Enums;

/// <summary>
/// Tipos de movimentação de estoque
/// </summary>
public enum StockMovementType
{
    Entry = 1,           // Entrada
    Exit = 2,            // Saída
    Adjustment = 3,      // Ajuste
    Transfer = 4,        // Transferência
    Sale = 5,            // Venda
    SaleReturn = 6,      // Devolução de venda
    PurchaseReturn = 7   // Devolução de compra
}

/// <summary>
/// Status de vendas
/// </summary>
public enum SaleStatus
{
    Open = 1,        // Aberta
    Completed = 2,   // Concluída
    Cancelled = 3,   // Cancelada
    Pending = 4      // Pendente
}

/// <summary>
/// Tipos de formas de pagamento
/// </summary>
public enum PaymentMethodType
{
    Cash = 1,              // Dinheiro
    Pix = 2,               // Pix
    DebitCard = 3,         // Débito
    CreditCard = 4,        // Crédito
    BankTransfer = 5,      // Transferência
    Installment = 6,       // Parcelado
    Boleto = 7             // Boleto
}

/// <summary>
/// Status de pagamento
/// </summary>
public enum PaymentStatus
{
    Pending = 1,      // Pendente
    Completed = 2,    // Concluído
    Cancelled = 3,    // Cancelado
    Refunded = 4      // Reembolsado
}

/// <summary>
/// Status de contas a receber/pagar
/// </summary>
public enum AccountStatus
{
    Pending = 1,      // Pendente
    Paid = 2,         // Pago
    Overdue = 3,      // Vencido
    Cancelled = 4,    // Cancelado
    PartiallyPaid = 5 // Parcialmente pago
}

/// <summary>
/// Tipos de unidades de medida
/// </summary>
public enum UnitType
{
    Un = 1,    // Unidade
    Kg = 2,    // Quilograma
    G = 3,     // Grama
    L = 4,     // Litro
    Ml = 5,    // Mililitro
    M = 6,     // Metro
    Cm = 7,    // Centímetro
    Box = 8,   // Caixa
    Pcte = 9   // Pacote
}

/// <summary>
/// Tipo de ação de auditoria
/// </summary>
public enum AuditActionType
{
    Create = 1,   // Criação
    Update = 2,   // Atualização
    Delete = 3,   // Exclusão
    View = 4      // Visualização
}

/// <summary>
/// Status de sessão de caixa
/// </summary>
public enum CashSessionStatus
{
    Open = 1,          // Aberta
    Closed = 2,        // Fechada
    PendingReview = 3  // Pendente revisão
}

/// <summary>
/// Tipo de movimentação de caixa
/// </summary>
public enum CashMovementType
{
    Sale = 1,        // Venda
    Supply = 2,      // Suprimento
    Withdrawal = 3,  // Saque
    Other = 4        // Outro
}

/// <summary>
/// Intervalo de recorrência
/// </summary>
public enum RecurrenceInterval
{
    Daily = 1,
    Weekly = 2,
    BiWeekly = 3,
    Monthly = 4,
    Quarterly = 5,
    Yearly = 6
}

/// <summary>
/// Categoria de despesa
/// </summary>
public enum ExpenseCategory
{
    Fuel = 1,
    Energy = 2,
    Water = 3,
    Internet = 4,
    Salary = 5,
    Rent = 6,
    Maintenance = 7,
    Food = 8,
    Taxes = 9,
    Other = 10
}
