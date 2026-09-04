using ERP.Domain.Entities;
using ERP.Infrastructure.Data.Contexts;
using ERP.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Infrastructure;

/// <summary>
/// Extensão para registrar serviços de infraestrutura no DI Container
/// </summary>
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
                sqlOptions.MigrationsAssembly("ERP.Infrastructure")));

        // Repositories
        services.AddScoped<IGenericRepository<Company>, GenericRepository<Company>>();
        services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
        services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
        services.AddScoped<IGenericRepository<PriceTable>, GenericRepository<PriceTable>>();
        services.AddScoped<IGenericRepository<Sale>, GenericRepository<Sale>>();
        services.AddScoped<IGenericRepository<Customer>, GenericRepository<Customer>>();
        
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPriceTableRepository, PriceTableRepository>();
        services.AddScoped<IProductPriceRepository, ProductPriceRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
