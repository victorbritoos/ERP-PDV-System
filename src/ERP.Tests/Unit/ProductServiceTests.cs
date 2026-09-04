namespace ERP.Tests.Unit;

using ERP.Application.DTOs.Product;
using ERP.Application.Services.Products;
using ERP.Domain.Entities;
using ERP.Domain.Exceptions;
using ERP.Infrastructure.Data.Repositories;
using Moq;
using AutoMapper;
using Xunit;
using FluentAssertions;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new ProductService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_ShouldCreateProduct()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var dto = new CreateProductDto
        {
            Sku = "TEST001",
            Name = "Test Product",
            CategoryId = Guid.NewGuid(),
            CostPrice = 100m
        };

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Sku = dto.Sku,
            Name = dto.Name,
            CompanyId = companyId,
            CreatedBy = userId
        };

        _repositoryMock.Setup(r => r.GetBySkuAsync(dto.Sku, companyId))
            .ReturnsAsync((Product)null!);
        
        _mapperMock.Setup(m => m.Map<Product>(dto))
            .Returns(product);
        
        _repositoryMock.Setup(r => r.AddAsync(product))
            .ReturnsAsync(product);
        
        _mapperMock.Setup(m => m.Map<ProductDto>(product))
            .Returns(new ProductDto { Id = product.Id, Name = product.Name, Sku = product.Sku });

        // Act
        var result = await _service.CreateAsync(dto, companyId, userId);

        // Assert
        result.Should().NotBeNull();
        result.Sku.Should().Be(dto.Sku);
        result.Name.Should().Be(dto.Name);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithExistingSku_ShouldThrowBusinessException()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var dto = new CreateProductDto
        {
            Sku = "EXISTING",
            Name = "Test Product",
            CategoryId = Guid.NewGuid(),
            CostPrice = 100m
        };

        var existingProduct = new Product { Sku = dto.Sku, CompanyId = companyId };

        _repositoryMock.Setup(r => r.GetBySkuAsync(dto.Sku, companyId))
            .ReturnsAsync(existingProduct);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessException>(
            () => _service.CreateAsync(dto, companyId, userId));
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var product = new Product { Id = productId, CompanyId = companyId, Name = "Test" };

        _repositoryMock.Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync(product);
        
        _mapperMock.Setup(m => m.Map<ProductDto>(product))
            .Returns(new ProductDto { Id = product.Id, Name = product.Name });

        // Act
        var result = await _service.GetByIdAsync(productId, companyId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(productId);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var companyId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync((Product)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.GetByIdAsync(productId, companyId));
    }
}
