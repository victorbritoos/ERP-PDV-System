namespace ERP.Shared.Constants;

/// <summary>
/// Constantes da aplicação
/// </summary>
public static class ApplicationConstants
{
    public static class Permissions
    {
        // Produtos
        public const string ProductsView = "Products.View";
        public const string ProductsCreate = "Products.Create";
        public const string ProductsEdit = "Products.Edit";
        public const string ProductsDelete = "Products.Delete";
        public const string ProductsImport = "Products.Import";
        public const string ProductsExport = "Products.Export";

        // Preços
        public const string PricesView = "Prices.View";
        public const string PricesEdit = "Prices.Edit";
        public const string PricesHistoryView = "Prices.HistoryView";
        public const string PriceTablesManage = "PriceTables.Manage";
        public const string PriceRulesManage = "PriceRules.Manage";

        // PDV/Vendas
        public const string PosOpen = "POS.Open";
        public const string PosSell = "POS.Sell";
        public const string PosCancelSale = "POS.CancelSale";
        public const string PosDiscount = "POS.Discount";
        public const string PosChangePriceTable = "POS.ChangePriceTable";
        public const string PosForcedPrice = "POS.ForcedPrice";

        // Estoque
        public const string InventoryView = "Inventory.View";
        public const string InventoryAdjust = "Inventory.Adjust";
        public const string InventoryTransfer = "Inventory.Transfer";
        public const string InventoryCount = "Inventory.Count";
        public const string InventoryImport = "Inventory.Import";
        public const string InventoryExport = "Inventory.Export";

        // Financeiro
        public const string FinanceView = "Finance.View";
        public const string FinanceReceive = "Finance.Receive";
        public const string FinancePay = "Finance.Pay";
        public const string FinanceReport = "Finance.Report";
        public const string FinanceExport = "Finance.Export";

        // Caixa
        public const string CashOpen = "Cash.Open";
        public const string CashClose = "Cash.Close";
        public const string CashMovement = "Cash.Movement";
        public const string CashReport = "Cash.Report";

        // Clientes
        public const string CustomersView = "Customers.View";
        public const string CustomersCreate = "Customers.Create";
        public const string CustomersEdit = "Customers.Edit";
        public const string CustomersDelete = "Customers.Delete";
        public const string CustomersCredit = "Customers.Credit";
        public const string CustomersReport = "Customers.Report";

        // Relatórios
        public const string ReportsView = "Reports.View";
        public const string ReportsExport = "Reports.Export";
        public const string ReportsSchedule = "Reports.Schedule";

        // Configurações
        public const string SettingsCompany = "Settings.Company";
        public const string SettingsSystem = "Settings.System";
        public const string SettingsUsers = "Settings.Users";
        public const string SettingsRoles = "Settings.Roles";
        public const string SettingsAudit = "Settings.Audit";
    }

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

    public static class DefaultUsers
    {
        public const string AdminEmail = "admin@erpsystem.com";
        public const string ManagerEmail = "manager@erpsystem.com";
        public const string CashierEmail = "cashier@erpsystem.com";
    }
}
