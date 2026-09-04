namespace ERP.Tests.Unit;

using ERP.Application.DTOs.Price;
using ERP.Application.Services.Prices;
using ERP.Domain.Entities;
using ERP.Domain.Exceptions;
using ERP.Infrastructure.Data.Repositories;
using Moq;
using Xunit;
using FluentAssertions;

public class PriceCalculationServiceTests
{
    private readonly Mock<IProductPriceRepository> _productPriceRepositoryMock;
    private readonly Mock<IPriceTableRepository> _priceTableRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly PriceCalculationService _service;

    public PriceCalculationServiceTests()
    {
        _productPriceRepositoryMock = new Mock<IProductPriceRepository>();
        _priceTableRepositoryMock = new Mock<IPriceTableRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();

        _service = new PriceCalculationService(
            _productPriceRepositoryMock.Object,
            _priceTableRepositoryMock.Object,
            _productRepositoryMock.Object,
            _customerRepositoryMock.Object);
    }

    [Fact]
    public async Task CalculatePriceAsync_WithValidRequest_ShouldReturnPrice()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var priceTableId = Guid.NewGuid();

        var request = new CalculatePriceDto
        {
            ProductId = productId,
            Quantity = 10,
            PaymentMethodType = 1,
            CustomerId = null,
            ForcedPriceTableId = null
        };

        var product = new Product { Id = productId, CompanyId = companyId, Name = "Test Product" };
        var priceTable = new PriceTable { Id = priceTableId, Name = "BASE", IsDefault = true, CompanyId = companyId };
        var productPrice = new ProductPrice { Id = Guid.NewGuid(), ProductId = productId, PriceTableId = priceTableId, UnitPrice = 100m };

        _productRepositoryMock.Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync(product);
        
        _priceTableRepositoryMock.Setup(r => r.GetDefaultAsync(companyId))
            .ReturnsAsync(priceTable);
        
        _productPriceRepositoryMock.Setup(r => r.GetCurrentPriceAsync(productId, priceTableId))
            .ReturnsAsync(productPrice);

        // Act
        var result = await _service.CalculatePriceAsync(request, companyId);

        // Assert
        result.Should().NotBeNull();
        result.ProductId.Should().Be(productId);
        result.UnitPrice.Should().Be(100m);
        result.Total.Should().Be(1000m); // 100 * 10
        result.PriceTableId.Should().Be(priceTableId);
    }

    [Fact]
    public async Task CalculatePriceAsync_WithNonExistentProduct_ShouldThrowNotFoundException()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var request = new CalculatePriceDto
        {
            ProductId = productId,
            Quantity = 10,
            PaymentMethodType = 1
        };

        _productRepositoryMock.Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync((Product)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.CalculatePriceAsync(request, companyId));
    }

    [Fact]
    public async Task CalculatePriceAsync_WithCustomerDefaultPriceTable_ShouldUseCustomerTable()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var customerPriceTableId = Guid.NewGuid();

        var request = new CalculatePriceDto
        {
            ProductId = productId,
            Quantity = 5,
            PaymentMethodType = 1,
            CustomerId = customerId,
            ForcedPriceTableId = null
        };

        var product = new Product { Id = productId, CompanyId = companyId, Name = "Test Product" };
        var customer = new Customer { Id = customerId, CompanyId = companyId, DefaultPriceTableId = customerPriceTableId };
        var priceTable = new PriceTable { Id = customerPriceTableId, Name = "CLIENTE_ESPECIAL", CompanyId = companyId };
        var productPrice = new ProductPrice { Id = Guid.NewGuid(), ProductId = productId, PriceTableId = customerPriceTableId, UnitPrice = 80m };

        _productRepositoryMock.Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync(product);
        
        _customerRepositoryMock.Setup(r => r.GetByIdAsync(customerId))
            .ReturnsAsync(customer);
        
        _productPriceRepositoryMock.Setup(r => r.GetCurrentPriceAsync(productId, customerPriceTableId))
            .ReturnsAsync(productPrice);
        
        _priceTableRepositoryMock.Setup(r => r.GetByIdAsync(customerPriceTableId))
            .ReturnsAsync(priceTable);

        // Act
        var result = await _service.CalculatePriceAsync(request, companyId);

        // Assert
        result.Should().NotBeNull();
        result.PriceTableId.Should().Be(customerPriceTableId);
        result.UnitPrice.Should().Be(80m);
        result.Total.Should().Be(400m); // 80 * 5
        result.AppliedRuleName.Should().Contain("cliente");
    }
}
