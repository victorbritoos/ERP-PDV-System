namespace ERP.API.Extensions;

using ERP.Shared.Constants;
using Microsoft.AspNetCore.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Política para Produtos
            options.AddPolicy("ProductsView", policy =>
                policy.RequireClaim("Permission", ApplicationConstants.Permissions.ProductsView));
            options.AddPolicy("ProductsCreate", policy =>
                policy.RequireClaim("Permission", ApplicationConstants.Permissions.ProductsCreate));
            options.AddPolicy("ProductsEdit", policy =>
                policy.RequireClaim("Permission", ApplicationConstants.Permissions.ProductsEdit));
            options.AddPolicy("ProductsDelete", policy =>
                policy.RequireClaim("Permission", ApplicationConstants.Permissions.ProductsDelete));

            // Política para Preços
            options.AddPolicy("PricesView", policy =>
                policy.RequireClaim("Permission", ApplicationConstants.Permissions.PricesView));
            options.AddPolicy("PricesEdit", policy =>
                policy.RequireClaim("Permission", ApplicationConstants.Permissions.PricesEdit));
            options.AddPolicy("PriceTablesManage", policy =>
                policy.RequireClaim("Permission", ApplicationConstants.Permissions.PriceTablesManage));

            // Política para PDV
            options.AddPolicy("PosOpen", policy =>
                policy.RequireClaim("Permission", ApplicationConstants.Permissions.PosOpen));
            options.AddPolicy("PosSell", policy =>
                policy.RequireClaim("Permission", ApplicationConstants.Permissions.PosSell));

            // Política para Clientes
            options.AddPolicy("CustomersView", policy =>
                policy.RequireClaim("Permission", ApplicationConstants.Permissions.CustomersView));
            options.AddPolicy("CustomersCreate", policy =>
                policy.RequireClaim("Permission", ApplicationConstants.Permissions.CustomersCreate));
            options.AddPolicy("CustomersEdit", policy =>
                policy.RequireClaim("Permission", ApplicationConstants.Permissions.CustomersEdit));

            // Política para Estoque
            options.AddPolicy("InventoryView", policy =>
                policy.RequireClaim("Permission", ApplicationConstants.Permissions.InventoryView));
            options.AddPolicy("InventoryAdjust", policy =>
                policy.RequireClaim("Permission", ApplicationConstants.Permissions.InventoryAdjust));
        });

        return services;
    }
}
