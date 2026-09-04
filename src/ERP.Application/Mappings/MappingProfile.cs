namespace ERP.Application.Mappings;

using AutoMapper;
using ERP.Application.DTOs.Customer;
using ERP.Application.DTOs.Price;
using ERP.Application.DTOs.Product;
using ERP.Application.DTOs.PriceTable;
using ERP.Application.DTOs.Sale;
using ERP.Domain.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Produtos
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name))
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand!.Name))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier!.LegalName))
            .ForMember(dest => dest.Prices, opt => opt.MapFrom(src => src.Prices));

        CreateMap<Product, ProductListDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name));

        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        // Preços de Produtos
        CreateMap<ProductPrice, ProductPriceDto>()
            .ForMember(dest => dest.PriceTableName, opt => opt.MapFrom(src => src.PriceTable!.Name));

        // Tabelas de Preço
        CreateMap<PriceTable, PriceTableDto>()
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.ProductPrices.Count));

        CreateMap<PriceTable, PriceTableListDto>();

        CreateMap<CreatePriceTableDto, PriceTable>();
        CreateMap<UpdatePriceTableDto, PriceTable>();

        // Clientes
        CreateMap<Customer, CustomerDto>()
            .ForMember(dest => dest.DefaultPriceTableName, opt => opt.MapFrom(src => src.DefaultPriceTable!.Name));

        CreateMap<Customer, CustomerListDto>();

        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();

        // Vendas
        CreateMap<Sale, SaleDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer!.Name))
            .ForMember(dest => dest.PriceTableName, opt => opt.MapFrom(src => src.PriceTable!.Name))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments));

        CreateMap<SaleItem, SaleItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.Name))
            .ForMember(dest => dest.ProductSku, opt => opt.MapFrom(src => src.Product!.Sku));

        CreateMap<SalePayment, SalePaymentDto>()
            .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(src => src.PaymentMethod!.Name));
    }
}
