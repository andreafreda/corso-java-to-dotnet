// UnitTests/OrderServiceTests.cs
using Moq;
using OrderService.Core.DTOs;
using OrderService.Core.Entities;
using OrderService.Core.Interfaces.Repositories;
using OrderService.Core.Services;

namespace OrderService.UnitTests;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _repoMock;
    private readonly Core.Services.OrderService _sut;

    public OrderServiceTests()
    {
        _repoMock = new Mock<IOrderRepository>();
        _sut      = new Core.Services.OrderService(_repoMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            new() { Id = 1, Customer = "Alice", Total = 100m },
            new() { Id = 2, Customer = "Bob",   Total = 200m }
        };
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(orders);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDto()
    {
        // Arrange
        var order = new Order { Id = 1, Customer = "Alice", Total = 100m };
        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(order);

        // Act
        var dto = await _sut.GetByIdAsync(1);

        // Assert
        Assert.NotNull(dto);
        Assert.Equal("Alice", dto.Customer);
        Assert.Equal(100m, dto.Total);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ReturnsNull()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Order?)null);

        // Act
        var dto = await _sut.GetByIdAsync(99);

        // Assert
        Assert.Null(dto);
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_CallsRepositoryAndReturnsDto()
    {
        // Arrange
        var request = new CreateOrderRequest("Alice", 150m);
        var saved   = new Order { Id = 42, Customer = "Alice", Total = 150m };

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(saved);

        // Act
        var dto = await _sut.CreateAsync(request);

        // Assert
        Assert.Equal(42, dto.Id);
        Assert.Equal("Alice", dto.Customer);
        _repoMock.Verify(r => r.AddAsync(
            It.Is<Order>(o => o.Customer == "Alice" && o.Total == 150m),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
