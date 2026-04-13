// NUnitTests/OrderServiceTests.cs
using Moq;
using OrderService.Core.DTOs;
using OrderService.Core.Entities;
using OrderService.Core.Interfaces.Repositories;

namespace OrderService.NUnitTests;

[TestFixture]
public class OrderServiceTests
{
    private Mock<IOrderRepository> _repo = null!;
    private Core.Services.OrderService _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _repo = new Mock<IOrderRepository>();
        _sut  = new Core.Services.OrderService(_repo.Object);
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            new() { Id = 1, Customer = "Alice", Total = 100m },
            new() { Id = 2, Customer = "Bob",   Total = 200m }
        };
        _repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
             .ReturnsAsync(orders);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetByIdAsync_ExistingId_ReturnsDto()
    {
        // Arrange
        var order = new Order { Id = 1, Customer = "Alice", Total = 100m };
        _repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
             .ReturnsAsync(order);

        // Act
        var dto = await _sut.GetByIdAsync(1);

        // Assert
        Assert.That(dto, Is.Not.Null);
        Assert.That(dto!.Customer, Is.EqualTo("Alice"));
        Assert.That(dto.Total, Is.EqualTo(100m));
    }

    [Test]
    public async Task GetByIdAsync_NotFound_ReturnsNull()
    {
        // Arrange
        _repo.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
             .ReturnsAsync((Order?)null);

        // Act
        var dto = await _sut.GetByIdAsync(99);

        // Assert
        Assert.That(dto, Is.Null);
    }

    [Test]
    public async Task CreateAsync_ValidRequest_CallsRepositoryAndReturnsDto()
    {
        // Arrange
        var request = new CreateOrderRequest("Alice", 150m);
        var saved   = new Order { Id = 42, Customer = "Alice", Total = 150m };

        _repo.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(saved);

        // Act
        var dto = await _sut.CreateAsync(request);

        // Assert
        Assert.That(dto.Id, Is.EqualTo(42));
        Assert.That(dto.Customer, Is.EqualTo("Alice"));
        _repo.Verify(r => r.AddAsync(
            It.Is<Order>(o => o.Customer == "Alice" && o.Total == 150m),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
