using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ERP.Shared.Constants;
using ERP.Infrastructure.Data.Contexts;

namespace ERP.API;

public class ApplicationDatabaseSeed
{
    public static async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        // Criar roles
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        var roles = new[] 
        { 
            ApplicationConstants.Roles.Administrator,
            ApplicationConstants.Roles.Manager,
            ApplicationConstants.Roles.Cashier,
            ApplicationConstants.Roles.Stockist,
            ApplicationConstants.Roles.Finance,
            ApplicationConstants.Roles.Salesman,
            ApplicationConstants.Roles.Viewer
        };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new Role { Name = roleName });
            }
        }

        // Criar empresa padrão
        if (!context.Companies.Any())
        {
            var company = new Company
            {
                Id = Guid.NewGuid(),
                Name = "ERP PDV System",
                TradingName = "Sistema ERP PDV",
                Document = "00.000.000/0000-00",
                City = "São Paulo",
                State = "SP",
                Email = "contato@erpsystem.com",
                Phone = "11 3000-0000",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Companies.Add(company);
            await context.SaveChangesAsync();

            // Criar usuários
            var users = new List<(string email, string password, string role)>
            {
                (ApplicationConstants.DefaultUsers.AdminEmail, "Admin@123456", ApplicationConstants.Roles.Administrator),
                (ApplicationConstants.DefaultUsers.ManagerEmail, "Manager@123456", ApplicationConstants.Roles.Manager),
                (ApplicationConstants.DefaultUsers.CashierEmail, "Cashier@123456", ApplicationConstants.Roles.Cashier)
            };

            foreach (var (email, password, role) in users)
            {
                if (!await userManager.FindByEmailAsync(email) is null)
                    continue;

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = email.Split('@')[0],
                    Email = email,
                    EmailConfirmed = true,
                    FullName = role switch
                    {
                        ApplicationConstants.Roles.Administrator => "Administrador",
                        ApplicationConstants.Roles.Manager => "Gerente",
                        ApplicationConstants.Roles.Cashier => "Caixa",
                        _ => "Usuário"
                    },
                    CompanyId = company.Id,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);

                    // Adicionar claims de permissão
                    if (role == ApplicationConstants.Roles.Administrator)
                    {
                        // Admin tem todas as permissões
                        var allPermissions = GetAllPermissions();
                        foreach (var permission in allPermissions)
                        {
                            await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Permission", permission));
                        }
                    }
                    else if (role == ApplicationConstants.Roles.Manager)
                    {
                        // Manager tem permissões gerenciais
                        var managerPermissions = GetManagerPermissions();
                        foreach (var permission in managerPermissions)
                        {
                            await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Permission", permission));
                        }
                    }
                    else if (role == ApplicationConstants.Roles.Cashier)
                    {
                        // Cashier tem permissões de PDV
                        var cashierPermissions = GetCashierPermissions();
                        foreach (var permission in cashierPermissions)
                        {
                            await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Permission", permission));
                        }
                    }
                }
            }

            // Criar formas de pagamento
            var paymentMethods = new[]
            {
                new PaymentMethod { Id = Guid.NewGuid(), Name = "Dinheiro", Code = "CASH", CompanyId = company.Id, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new PaymentMethod { Id = Guid.NewGuid(), Name = "Pix", Code = "PIX", CompanyId = company.Id, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new PaymentMethod { Id = Guid.NewGuid(), Name = "Débito", Code = "DEBIT", CompanyId = company.Id, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new PaymentMethod { Id = Guid.NewGuid(), Name = "Crédito", Code = "CREDIT", CompanyId = company.Id, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };
            context.PaymentMethods.AddRange(paymentMethods);

            // Criar tabelas de preço
            var priceTables = new[]
            {
                new PriceTable { Id = Guid.NewGuid(), Name = "BASE", Description = "Tabela Base", Priority = 5, IsDefault = true, CompanyId = company.Id, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new PriceTable { Id = Guid.NewGuid(), Name = "DINHEIRO", Description = "Tabela para Pagamento em Dinheiro", Priority = 4, IsDefault = false, CompanyId = company.Id, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new PriceTable { Id = Guid.NewGuid(), Name = "PIX", Description = "Tabela para Pagamento em Pix", Priority = 3, IsDefault = false, CompanyId = company.Id, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new PriceTable { Id = Guid.NewGuid(), Name = "ATACADO", Description = "Tabela para Venda Atacado", Priority = 2, IsDefault = false, CompanyId = company.Id, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new PriceTable { Id = Guid.NewGuid(), Name = "REVENDEDOR", Description = "Tabela para Revendedor", Priority = 1, IsDefault = false, CompanyId = company.Id, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };
            context.PriceTables.AddRange(priceTables);

            // Criar categorias
            var categories = new[]
            {
                new Category { Id = Guid.NewGuid(), Name = "Alimentação", DisplayOrder = 1, CompanyId = company.Id, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Category { Id = Guid.NewGuid(), Name = "Bebidas", DisplayOrder = 2, CompanyId = company.Id, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Category { Id = Guid.NewGuid(), Name = "Limpeza", DisplayOrder = 3, CompanyId = company.Id, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };
            context.Categories.AddRange(categories);

            // Criar caixa
            var cashRegister = new CashRegister
            {
                Id = Guid.NewGuid(),
                Name = "Caixa 1",
                CompanyId = company.Id,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.CashRegisters.Add(cashRegister);

            await context.SaveChangesAsync();
        }
    }

    private static List<string> GetAllPermissions()
    {
        var permissions = new List<string>
        {
            ApplicationConstants.Permissions.ProductsView,
            ApplicationConstants.Permissions.ProductsCreate,
            ApplicationConstants.Permissions.ProductsEdit,
            ApplicationConstants.Permissions.ProductsDelete,
            ApplicationConstants.Permissions.ProductsImport,
            ApplicationConstants.Permissions.ProductsExport,
            ApplicationConstants.Permissions.PricesView,
            ApplicationConstants.Permissions.PricesEdit,
            ApplicationConstants.Permissions.PricesHistoryView,
            ApplicationConstants.Permissions.PriceTablesManage,
            ApplicationConstants.Permissions.PriceRulesManage,
            ApplicationConstants.Permissions.PosOpen,
            ApplicationConstants.Permissions.PosSell,
            ApplicationConstants.Permissions.PosCancelSale,
            ApplicationConstants.Permissions.PosDiscount,
            ApplicationConstants.Permissions.PosChangePriceTable,
            ApplicationConstants.Permissions.PosForcedPrice,
            ApplicationConstants.Permissions.InventoryView,
            ApplicationConstants.Permissions.InventoryAdjust,
            ApplicationConstants.Permissions.InventoryTransfer,
            ApplicationConstants.Permissions.InventoryCount,
            ApplicationConstants.Permissions.InventoryImport,
            ApplicationConstants.Permissions.InventoryExport,
            ApplicationConstants.Permissions.FinanceView,
            ApplicationConstants.Permissions.FinanceReceive,
            ApplicationConstants.Permissions.FinancePay,
            ApplicationConstants.Permissions.FinanceReport,
            ApplicationConstants.Permissions.FinanceExport,
            ApplicationConstants.Permissions.CashOpen,
            ApplicationConstants.Permissions.CashClose,
            ApplicationConstants.Permissions.CashMovement,
            ApplicationConstants.Permissions.CashReport,
            ApplicationConstants.Permissions.CustomersView,
            ApplicationConstants.Permissions.CustomersCreate,
            ApplicationConstants.Permissions.CustomersEdit,
            ApplicationConstants.Permissions.CustomersDelete,
            ApplicationConstants.Permissions.CustomersCredit,
            ApplicationConstants.Permissions.CustomersReport,
            ApplicationConstants.Permissions.ReportsView,
            ApplicationConstants.Permissions.ReportsExport,
            ApplicationConstants.Permissions.ReportsSchedule,
            ApplicationConstants.Permissions.SettingsCompany,
            ApplicationConstants.Permissions.SettingsSystem,
            ApplicationConstants.Permissions.SettingsUsers,
            ApplicationConstants.Permissions.SettingsRoles,
            ApplicationConstants.Permissions.SettingsAudit
        };
        return permissions;
    }

    private static List<string> GetManagerPermissions()
    {
        return new List<string>
        {
            ApplicationConstants.Permissions.ProductsView,
            ApplicationConstants.Permissions.ProductsCreate,
            ApplicationConstants.Permissions.ProductsEdit,
            ApplicationConstants.Permissions.ProductsExport,
            ApplicationConstants.Permissions.PricesView,
            ApplicationConstants.Permissions.PricesEdit,
            ApplicationConstants.Permissions.PricesHistoryView,
            ApplicationConstants.Permissions.PriceTablesManage,
            ApplicationConstants.Permissions.PriceRulesManage,
            ApplicationConstants.Permissions.PosOpen,
            ApplicationConstants.Permissions.PosSell,
            ApplicationConstants.Permissions.PosDiscount,
            ApplicationConstants.Permissions.PosChangePriceTable,
            ApplicationConstants.Permissions.InventoryView,
            ApplicationConstants.Permissions.InventoryAdjust,
            ApplicationConstants.Permissions.InventoryTransfer,
            ApplicationConstants.Permissions.InventoryExport,
            ApplicationConstants.Permissions.FinanceView,
            ApplicationConstants.Permissions.FinanceReport,
            ApplicationConstants.Permissions.CashOpen,
            ApplicationConstants.Permissions.CashClose,
            ApplicationConstants.Permissions.CashReport,
            ApplicationConstants.Permissions.CustomersView,
            ApplicationConstants.Permissions.CustomersCreate,
            ApplicationConstants.Permissions.CustomersEdit,
            ApplicationConstants.Permissions.CustomersReport,
            ApplicationConstants.Permissions.ReportsView,
            ApplicationConstants.Permissions.ReportsExport,
            ApplicationConstants.Permissions.SettingsAudit
        };
    }

    private static List<string> GetCashierPermissions()
    {
        return new List<string>
        {
            ApplicationConstants.Permissions.ProductsView,
            ApplicationConstants.Permissions.PosOpen,
            ApplicationConstants.Permissions.PosSell,
            ApplicationConstants.Permissions.PosCancelSale,
            ApplicationConstants.Permissions.PosDiscount,
            ApplicationConstants.Permissions.PosChangePriceTable,
            ApplicationConstants.Permissions.CashOpen,
            ApplicationConstants.Permissions.CashClose,
            ApplicationConstants.Permissions.CashMovement,
            ApplicationConstants.Permissions.CashReport,
            ApplicationConstants.Permissions.CustomersView,
            ApplicationConstants.Permissions.PricesView
        };
    }
}
