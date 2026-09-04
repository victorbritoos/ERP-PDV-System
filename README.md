# ERP + PDV System

Sistema ERP profissional integrado com PDV (Ponto de Venda), desenvolvido em **.NET 8** com arquitetura em camadas, autenticação segura, múltiplas tabelas de preço e motor flexível de regras.

## 🎯 Características Principais

### ✅ Funcionalidades Implementadas (Roadmap)

- **PDV (Ponto de Venda)**: Interface rápida e intuitiva para vendas
- **Produtos & Categorias**: Gestão completa de catálogo
- **Múltiplas Tabelas de Preço**: Um produto, N preços diferentes
- **Motor de Regras de Preço**: Aplicação automática de preços baseada em condições
- **Clientes**: Cadastro com tabela de preço padrão
- **Estoque**: Movimentações, balanço, inventário
- **Pedidos & Orçamentos**: Fluxo completo de vendas
- **Caixa**: Abertura, movimentos, fechamento com balanço
- **Financeiro**: Contas a receber, contas a pagar, despesas
- **Relatórios**: Dashboard e relatórios diversos
- **Autenticação & Permissões**: Roles e permissões granulares
- **Auditoria**: Registro de todas as operações importantes
- **PDF**: Geração de documentos (venda, pedido, relatórios)

## 🏗️ Arquitetura

```
ERP.sln
├── ERP.Domain/                # Entidades, Value Objects, Enums
├── ERP.Application/           # Casos de uso, DTOs, Serviços
├── ERP.Infrastructure/        # EF Core, DbContext, Repositories
├── ERP.API/                   # Controllers, Middleware, Swagger
├── ERP.Web/                   # Blazor Web App (Frontend)
├── ERP.Tests/                 # xUnit, Testes Unitários e Integração
└── ERP.Shared/                # Enums, Constants, Extensions
```

### Princípios Arquiteturais

- **Arquitetura em Camadas**: Separação clara de responsabilidades
- **Repository Pattern**: Acesso a dados centralizado
- **Dependency Injection**: Inversão de controle via ASP.NET Core
- **SOLID & Clean Code**: Código limpo e manutenível
- **Transações Explícitas**: Operações críticas com rollback
- **Async/Await**: Todo fluxo de I/O assíncrono
- **DTOs**: Separação entre domínio e apresentação

## 🛠️ Stack Tecnológica

### Backend
- **.NET 8** (LTS)
- **ASP.NET Core** (REST API)
- **Entity Framework Core 8** (ORM)
- **SQL Server** (Database)
- **Serilog** (Logging)
- **FluentValidation** (Validações)
- **AutoMapper** (DTO Mapping)
- **JWT** (Autenticação)

### Frontend
- **Blazor Web App** (Components interativos)
- **HTML5 + CSS3** (Markup e estilos)
- **Bootstrap 5** (Layout responsivo)
- **JavaScript** (Interoperabilidade)

### Testes
- **xUnit** (Test Framework)
- **Moq** (Mocking)
- **FluentAssertions** (Assertions)

## 📋 Pré-requisitos

- **.NET 8 SDK** ou superior
- **Visual Studio 2022** (Community, Professional ou Enterprise)
- **SQL Server 2019** ou superior (LocalDB, Express ou Full)
- **Git** para versionamento

## 🚀 Instalação e Execução

### 1. Clonar o repositório

```bash
git clone https://github.com/victorbritoos/ERP-PDV-System.git
cd ERP-PDV-System
```

### 2. Configurar banco de dados

#### a) Abrir appsettings.json em `ERP.API` e verificar connection string

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ErpPdvDb;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

#### b) Abrir Package Manager Console no Visual Studio

```powershell
# Defina o projeto como padrão: ERP.Infrastructure
Set-Location "src\ERP.Infrastructure"

# Criar ou atualizar banco
Update-Database
```

### 3. Executar aplicação

#### Opção A: Visual Studio
- Abrir solução `ERP.sln`
- Definir `ERP.API` como projeto de inicialização
- Pressionar `F5`

#### Opção B: CLI

```bash
cd src/ERP.API
dotnet run
```

### 4. Acessar a aplicação

- **API**: https://localhost:5001/api (Swagger disponível)
- **Web App**: https://localhost:5000 (Blazor)

## 🔐 Autenticação Padrão

Os usuários abaixo são criados automaticamente via Seed:

```
Email: admin@erpsystem.com
Senha: Admin@123456
Role: Administrator

Email: manager@erpsystem.com
Senha: Manager@123456
Role: Manager

Email: cashier@erpsystem.com
Senha: Cashier@123456
Role: Cashier
```

## 📊 Estrutura do Banco de Dados

### Entidades Principais

- **Companies**: Multiempresa
- **Users & Roles**: Autenticação e autorização
- **Products**: Catálogo de produtos
- **Categories**: Categorização de produtos
- **Suppliers**: Fornecedores
- **Customers**: Clientes
- **PriceTables**: Tabelas de preço (BASE, DINHEIRO, PIX, ATACADO, etc)
- **ProductPrices**: Preços de produtos por tabela
- **PriceRules**: Motor de regras de preço
- **Sales**: Vendas realizadas
- **StockMovements**: Movimentações de estoque
- **CashRegisterSessions**: Sessões de caixa
- **AccountsReceivable & AccountsPayable**: Contas financeiras
- **AuditLogs**: Auditoria de operações

### Diagrama de Relacionamentos

```
Companies
    ├─→ Users (1:N)
    ├─→ Products (1:N)
    ├─→ Customers (1:N)
    ├─→ PriceTables (1:N)
    ├─→ Sales (1:N)
    └─→ AuditLogs (1:N)

Products
    ├─→ ProductPrices (1:N) ← CORE
    ├─→ Category (N:1)
    └─→ Supplier (N:1)

ProductPrices
    ├─→ PriceTable (N:1) ← CORE
    └─→ PriceHistory (1:N)

PriceRules
    ├─→ PriceRuleConditions (1:N)
    └─→ PriceRuleActions (1:N)

Sales
    ├─→ SaleItems (1:N)
    ├─→ SalePayments (1:N)
    ├─→ Customer (N:1)
    └─→ PriceTable (N:1)
```

## 🔄 Fluxos Principais

### Motor de Preços (DIFERENCIAL DO SISTEMA)

```
1. Buscar preço para um produto
   ↓
2. Avaliar regras por prioridade
   ↓
3. Se regra aplicável → usar tabela determinada
   ↓
4. Se nenhuma regra → usar tabela do cliente ou padrão
   ↓
5. Retornar preço com explicação
```

### Fluxo Completo de Venda

```
ABRIR PDV
  ├─ Selecionar cliente (tabela padrão)
  ├─ Selecionar/alterar tabela de preço
  ├─ Adicionar produtos (código de barras ou busca)
  ├─ Recalcular preços automaticamente
  ├─ Aplicar desconto
  ├─ Confirmar venda
  │  ├─ BEGIN TRANSACTION
  │  ├─ Criar Sale + SaleItems
  │  ├─ Registrar pagamento
  │  ├─ Atualizar estoque
  │  ├─ Registrar caixa
  │  ├─ COMMIT
  │  └─ Gerar PDF/Recibo
  └─ Finalizar
```

## 📁 Estrutura de Pastas

```
src/
├── ERP.Domain/
│   ├── Entities/
│   ├── Enums/
│   ├── ValueObjects/
│   ├── Events/
│   └── Constants.cs
│
├── ERP.Application/
│   ├── DTOs/
│   ├── Services/
│   ├── Validations/
│   ├── Mappings/
│   ├── UseCases/
│   ├── Interfaces/
│   └── ApplicationDependencyInjection.cs
│
├── ERP.Infrastructure/
│   ├── Data/
│   │   ├── Contexts/
│   │   ├── Repositories/
│   │   └── Migrations/
│   ├── Services/
│   ├── External/
│   └── InfrastructureDependencyInjection.cs
│
├── ERP.API/
│   ├── Controllers/
│   ├── Middleware/
│   ├── Extensions/
│   ├── appsettings.json
│   └── Program.cs
│
├── ERP.Web/
│   ├── Pages/
│   ├── Components/
│   ├── Layouts/
│   └── Services/
│
└── ERP.Tests/
    ├── Unit/
    ├── Integration/
    └── Fixtures/
```

## 🧪 Testes

Executar testes via linha de comando:

```bash
cd src/ERP.Tests
dotnet test
```

Ou via Visual Studio:
- Test Explorer → Run All Tests

## 📚 Documentação

- **Swagger/OpenAPI**: https://localhost:5001/swagger (quando executando)
- **Guia de Permissões**: Ver `PERMISSIONS.md`
- **Guia de Desenvolvimento**: Ver `DEVELOPMENT.md`

## 🤝 Contribuindo

1. Criar branch a partir de `develop`
2. Fazer commits descritivos
3. Abrir Pull Request
4. Verificar que testes passam
5. Aguardar review

## 📝 Fases de Implementação

- [x] FASE 1: Arquitetura e Estrutura
- [ ] FASE 2: Banco de Dados e Migrations
- [ ] FASE 3: Autenticação e Permissões
- [ ] FASE 4: Produtos
- [ ] FASE 5: Categorias e Fornecedores
- [ ] FASE 6: Motor de Tabelas de Preço
- [ ] FASE 7: Motor de Regras de Preço
- [ ] FASE 8: Clientes
- [ ] FASE 9: Estoque
- [ ] FASE 10: PDV
- [ ] FASE 11: Pedidos e Orçamentos
- [ ] FASE 12: Caixa
- [ ] FASE 13: Financeiro
- [ ] FASE 14: Relatórios
- [ ] FASE 15: Dashboard
- [ ] FASE 16-20: Refinamentos e Testes

## 📄 Licença

MIT License - veja LICENSE.md para detalhes

## 📧 Contato

Para dúvidas ou sugestões, abra uma issue no repositório.

---

**Desenvolvido com ❤️ em .NET**
