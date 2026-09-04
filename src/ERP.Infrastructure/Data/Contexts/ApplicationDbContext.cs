namespace ERP.Infrastructure.Data.Contexts;

using ERP.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// DbContext principal da aplicação - gerencia todas as entidades
/// </summary>
public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    #region Companies
    public DbSet<Company> Companies { get; set; }
    #endregion

    #region Products
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    #endregion

    #region Prices
    public DbSet<PriceTable> PriceTables { get; set; }
    public DbSet<ProductPrice> ProductPrices { get; set; }
    public DbSet<PriceHistory> PriceHistories { get; set; }
    public DbSet<PriceRule> PriceRules { get; set; }
    public DbSet<PriceRuleCondition> PriceRuleConditions { get; set; }
    public DbSet<PriceRuleAction> PriceRuleActions { get; set; }
    public DbSet<CustomerPriceTable> CustomerPriceTables { get; set; }
    #endregion

    #region Customers
    public DbSet<Customer> Customers { get; set; }
    #endregion

    #region Sales
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }
    public DbSet<SalePayment> SalePayments { get; set; }
    public DbSet<SaleCancellation> SaleCancellations { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    #endregion

    #region Inventory
    public DbSet<StockMovement> StockMovements { get; set; }
    public DbSet<InventoryAdjustment> InventoryAdjustments { get; set; }
    #endregion

    #region Cash
    public DbSet<CashRegister> CashRegisters { get; set; }
    public DbSet<CashRegisterSession> CashRegisterSessions { get; set; }
    public DbSet<CashRegisterMovement> CashRegisterMovements { get; set; }
    #endregion

    #region Finance
    public DbSet<AccountsReceivable> AccountsReceivable { get; set; }
    public DbSet<AccountsReceivablePayment> AccountsReceivablePayments { get; set; }
    public DbSet<AccountsPayable> AccountsPayable { get; set; }
    public DbSet<AccountsPayablePayment> AccountsPayablePayments { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    #endregion

    #region Audit
    public DbSet<AuditLog> AuditLogs { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Aplicar todas as configurações de entidades
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Configuração do Identity
        builder.Entity<Role>().ToTable("AspNetRoles");
        builder.Entity<User>().ToTable("AspNetUsers");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            if (entry.Entity is BaseEntity entity)
            {
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }

                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}

/// <summary>
/// Role para Identity
/// </summary>
public class Role : Microsoft.AspNetCore.Identity.IdentityRole<Guid>
{
    public string? Description { get; set; }
}
