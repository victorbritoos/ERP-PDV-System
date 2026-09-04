namespace ERP.Shared.Constants;

/// <summary>
/// Constantes da aplicação
/// </summary>
public static class ApplicationConstants
{
    /// <summary>
    /// Roles disponíveis
    /// </summary>
    public static class Roles
    {
        public const string Administrator = "Administrator";
        public const string Manager = "Manager";
        public const string Cashier = "Cashier";
        public const string Stockist = "Stockist";
        public const string Finance = "Finance";
        public const string Salesman = "Salesman";
        public const string Viewer = "Viewer";
    }

    /// <summary>
    /// Permissões granulares
    /// </summary>
    public static class Permissions
    {
        // Produtos
        public const string ProductsView = "products.view";
        public const string ProductsCreate = "products.create";
        public const string ProductsEdit = "products.edit";
        public const string ProductsDelete = "products.delete";
        public const string ProductsImport = "products.import";
        public const string ProductsExport = "products.export";

        // Preços
        public const string PricesView = "prices.view";
        public const string PricesEdit = "prices.edit";
        public const string PricesHistoryView = "prices.history.view";
        public const string PriceTablesManage = "price_tables.manage";
        public const string PriceRulesManage = "price_rules.manage";

        // PDV
        public const string PosOpen = "pos.open";
        public const string PosSell = "pos.sell";
        public const string PosCancelSale = "pos.cancel_sale";
        public const string PosDiscount = "pos.discount";
        public const string PosChangePriceTable = "pos.change_price_table";
        public const string PosForcedPrice = "pos.forced_price";

        // Estoque
        public const string InventoryView = "inventory.view";
        public const string InventoryAdjust = "inventory.adjust";
        public const string InventoryTransfer = "inventory.transfer";
        public const string InventoryCount = "inventory.count";
        public const string InventoryImport = "inventory.import";
        public const string InventoryExport = "inventory.export";

        // Financeiro
        public const string FinanceView = "finance.view";
        public const string FinanceReceive = "finance.receive";
        public const string FinancePay = "finance.pay";
        public const string FinanceReport = "finance.report";
        public const string FinanceExport = "finance.export";

        // Caixa
        public const string CashOpen = "cash.open";
        public const string CashClose = "cash.close";
        public const string CashMovement = "cash.movement";
        public const string CashReport = "cash.report";

        // Clientes
        public const string CustomersView = "customers.view";
        public const string CustomersCreate = "customers.create";
        public const string CustomersEdit = "customers.edit";
        public const string CustomersDelete = "customers.delete";
        public const string CustomersCredit = "customers.credit";
        public const string CustomersReport = "customers.report";

        // Relatórios
        public const string ReportsView = "reports.view";
        public const string ReportsExport = "reports.export";
        public const string ReportsSchedule = "reports.schedule";

        // Configurações
        public const string SettingsCompany = "settings.company";
        public const string SettingsSystem = "settings.system";
        public const string SettingsUsers = "settings.users";
        public const string SettingsRoles = "settings.roles";
        public const string SettingsAudit = "settings.audit";
    }

    /// <summary>
    /// Usuários padrão
    /// </summary>
    public static class DefaultUsers
    {
        public const string AdminEmail = "admin@erpsystem.com";
        public const string ManagerEmail = "manager@erpsystem.com";
        public const string CashierEmail = "cashier@erpsystem.com";
    }

    /// <summary>
    /// Configurações de aplicação
    /// </summary>
    public static class Settings
    {
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 100;
        public const int PriceDecimalPlaces = 2;
        public const int QuantityDecimalPlaces = 4;
    }
}
