# Sistema de Permissões

## Visão Geral

O sistema utiliza um modelo baseado em **Claims** do ASP.NET Core Identity, permitindo granularidade máxima e flexibilidade.

## Estrutura

### Roles (Papéis)

```
- Administrator: Acesso total ao sistema
- Manager: Gerenciamento geral, permissões variadas
- Cashier: Operações de caixa e vendas
- Stockist: Gerenciamento de estoque
- Finance: Operações financeiras
- Salesman: Acesso restrito a vendas
- Viewer: Apenas visualização de dados
```

### Permissões Granulares

#### Produtos
```
Products.View          - Visualizar produtos
Products.Create        - Criar novo produto
Products.Edit          - Editar produto existente
Products.Delete        - Deletar produto
Products.Import        - Importar produtos em massa
Products.Export        - Exportar lista de produtos
```

#### Preços
```
Prices.View            - Visualizar preços
Prices.Edit            - Editar preços
Prices.HistoryView     - Ver histórico de preços
PriceTables.Manage     - Gerenciar tabelas de preço
PriceRules.Manage      - Gerenciar regras de preço
```

#### Vendas / PDV
```
POS.Open               - Abrir e operar PDV
POS.Sell               - Realizar vendas
POS.CancelSale         - Cancelar vendas
POS.Discount           - Aplicar descontos
POS.ChangePriceTable   - Alterar tabela de preço
POS.ForcedPrice        - Forçar preço manual
```

#### Estoque
```
Inventory.View         - Visualizar estoque
Inventory.Adjust       - Fazer ajustes de estoque
Inventory.Transfer     - Transferir entre depósitos
Inventory.Count        - Realizar contagem/inventário
Inventory.Import       - Importar estoque
Inventory.Export       - Exportar relatório
```

#### Financeiro
```
Finance.View           - Visualizar dados financeiros
Finance.Receive        - Receber contas a receber
Finance.Pay            - Pagar contas a pagar
Finance.Report         - Gerar relatórios financeiros
Finance.Export         - Exportar dados financeiros
```

#### Caixa
```
Cash.Open              - Abrir caixa
Cash.Close             - Fechar caixa
Cash.Movement          - Registrar movimentos
Cash.Report            - Ver relatório de caixa
```

#### Clientes
```
Customers.View         - Visualizar clientes
Customers.Create       - Criar cliente
Customers.Edit         - Editar cliente
Customers.Delete       - Deletar cliente
Customers.Credit       - Gerenciar crédito
Customers.Report       - Relatório de clientes
```

#### Relatórios
```
Reports.View           - Acessar relatórios
Reports.Export         - Exportar relatórios
Reports.Schedule       - Agendar relatórios
```

#### Configurações
```
Settings.Company       - Configurar dados da empresa
Settings.System        - Configurar sistema
Settings.Users         - Gerenciar usuários
Settings.Roles         - Gerenciar roles
Settings.Audit         - Visualizar auditoria
```

## Atribuição de Permissões

### Por Papel

Cada role recebe um conjunto padrão de permissões:

```csharp
// Administrator
- Todas as permissões

// Manager
- Products.*, Prices.*, POS.*, Inventory.*, Finance.*, Customers.*, Reports.*
- Não pode: Settings.System, Settings.Roles

// Cashier
- POS.Open, POS.Sell, POS.CancelSale (com restrições), POS.Discount, POS.ChangePriceTable
- Cash.Open, Cash.Close, Cash.Movement, Cash.Report
- Customers.View
- Products.View (apenas consulta)

// Stockist
- Inventory.View, Inventory.Adjust, Inventory.Transfer, Inventory.Count
- Inventory.Import, Inventory.Export
- Products.View
- Reports.View (estoque)

// Finance
- Finance.* (todas operações financeiras)
- Customers.View, AccountsReceivable.View
- Reports.Export
- Settings.Audit

// Salesman
- POS.Sell
- Customers.View
- Products.View
- Reports.View (vendas apenas)

// Viewer
- *.View (apenas leitura em tudo)
```

### Personalizado

Administrador pode criar roles personalizadas e atribuir permissões específicas por usuário.

## Validação de Permissões

### No Backend (Controllers/Services)

```csharp
[HttpPost("products")]
[Authorize(Policy = "Products.Create")]
public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
{
    // Lógica...
}
```

### No Frontend (Blazor)

```razor
@if (User.HasClaim("Permission", "Products.Create"))
{
    <button @onclick="OpenCreateDialog">Novo Produto</button>
}
```

## Auditoria de Permissões

Todas as operações sensíveis são auditadas:
- Quem executou
- Quando executou
- O quê foi executado
- Resultado (sucesso/falha)
- IP de origem

## Segurança

### Princípios

1. **Principle of Least Privilege**: Usuários recebem apenas permissões necessárias
2. **Deny by Default**: Se permissão não é explícita, é negada
3. **Audit Everything**: Todas operações críticas são registradas
4. **Role Separation**: Roles são bem definidos e separados

### Validação em Dois Níveis

1. **Autenticação**: Verificar identidade (quem é?)
2. **Autorização**: Verificar permissões (pode fazer?)

---

**Última atualização**: Setembro 2026
