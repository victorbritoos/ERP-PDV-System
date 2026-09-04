namespace ERP.Application.Services.Products;

using AutoMapper;
using ERP.Application.DTOs.Product;
using ERP.Domain.Entities;
using ERP.Domain.Exceptions;
using ERP.Infrastructure.Data.Repositories;

/// <summary>
/// Serviço de gerenciamento de produtos
/// </summary>
public interface IProductService
{
    Task<ProductDto> CreateAsync(CreateProductDto dto, Guid companyId, Guid userId);
    Task<ProductDto> GetByIdAsync(Guid productId, Guid companyId);
    Task<ProductDto?> GetByBarcodeAsync(string barcode, Guid companyId);
    Task<IEnumerable<ProductListDto>> GetAllAsync(Guid companyId, int page = 1, int pageSize = 10);
    Task<IEnumerable<ProductListDto>> SearchAsync(string searchTerm, Guid companyId, int page = 1, int pageSize = 10);
    Task<ProductDto> UpdateAsync(Guid productId, UpdateProductDto dto, Guid companyId, Guid userId);
    Task DeleteAsync(Guid productId, Guid companyId, Guid userId);
}

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto, Guid companyId, Guid userId)
    {
        // Validar se SKU já existe
        var existingSku = await _repository.GetBySkuAsync(dto.Sku, companyId);
        if (existingSku != null)
            throw new BusinessException("Produto com este SKU já existe", "SKU_ALREADY_EXISTS");

        // Validar se código de barras já existe (se fornecido)
        if (!string.IsNullOrEmpty(dto.Barcode))
        {
            var existingBarcode = await _repository.GetByBarcodeAsync(dto.Barcode, companyId);
            if (existingBarcode != null)
                throw new BusinessException("Produto com este código de barras já existe", "BARCODE_ALREADY_EXISTS");
        }

        // Criar produto
        var product = _mapper.Map<Product>(dto);
        product.CompanyId = companyId;
        product.CreatedBy = userId;
        product.UpdatedBy = userId;
        product.CurrentStock = 0;

        var createdProduct = await _repository.AddAsync(product);
        return _mapper.Map<ProductDto>(createdProduct);
    }

    public async Task<ProductDto> GetByIdAsync(Guid productId, Guid companyId)
    {
        var product = await _repository.GetByIdAsync(productId);
        if (product == null || product.CompanyId != companyId)
            throw new NotFoundException($"Produto não encontrado");

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto?> GetByBarcodeAsync(string barcode, Guid companyId)
    {
        var product = await _repository.GetByBarcodeAsync(barcode, companyId);
        if (product == null)
            return null;

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<IEnumerable<ProductListDto>> GetAllAsync(Guid companyId, int page = 1, int pageSize = 10)
    {
        var products = await _repository.GetAllAsync(companyId, page, pageSize);
        return _mapper.Map<IEnumerable<ProductListDto>>(products);
    }

    public async Task<IEnumerable<ProductListDto>> SearchAsync(string searchTerm, Guid companyId, int page = 1, int pageSize = 10)
    {
        var products = await _repository.SearchAsync(searchTerm, companyId, page, pageSize);
        return _mapper.Map<IEnumerable<ProductListDto>>(products);
    }

    public async Task<ProductDto> UpdateAsync(Guid productId, UpdateProductDto dto, Guid companyId, Guid userId)
    {
        var product = await _repository.GetByIdAsync(productId);
        if (product == null || product.CompanyId != companyId)
            throw new NotFoundException($"Produto não encontrado");

        _mapper.Map(dto, product);
        product.UpdatedBy = userId;
        product.UpdatedAt = DateTime.UtcNow;

        var updatedProduct = await _repository.UpdateAsync(product);
        return _mapper.Map<ProductDto>(updatedProduct);
    }

    public async Task DeleteAsync(Guid productId, Guid companyId, Guid userId)
    {
        var product = await _repository.GetByIdAsync(productId);
        if (product == null || product.CompanyId != companyId)
            throw new NotFoundException($"Produto não encontrado");

        product.IsActive = false;
        product.UpdatedBy = userId;
        product.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(product);
    }
}
