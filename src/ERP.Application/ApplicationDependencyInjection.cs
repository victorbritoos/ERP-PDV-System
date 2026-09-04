using ERP.Application.Mappings;
using ERP.Application.Services.Customers;
using ERP.Application.Services.Prices;
using ERP.Application.Services.PriceTables;
using ERP.Application.Services.Products;
using ERP.Application.Validators.Customer;
using ERP.Application.Validators.Product;
using ERP.Application.Validators.PriceTable;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Application;

/// <summary>
/// Extensão para registrar serviços de aplicação no DI Container
/// </summary>
public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // FluentValidation
        services.AddScoped<IValidator<CreateProductDto>, CreateProductValidator>();
        services.AddScoped<IValidator<UpdateProductDto>, UpdateProductValidator>();
        services.AddScoped<IValidator<CreatePriceTableDto>, CreatePriceTableValidator>();
        services.AddScoped<IValidator<UpdatePriceTableDto>, UpdatePriceTableValidator>();
        services.AddScoped<IValidator<CreateCustomerDto>, CreateCustomerValidator>();
        services.AddScoped<IValidator<UpdateCustomerDto>, UpdateCustomerValidator>();

        // Application Services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IPriceTableService, PriceTableService>();
        services.AddScoped<IPriceCalculationService, PriceCalculationService>();
        services.AddScoped<ICustomerService, CustomerService>();

        return services;
    }
}
