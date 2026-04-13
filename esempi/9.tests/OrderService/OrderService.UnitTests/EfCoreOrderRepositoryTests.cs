// UnitTests/EfCoreOrderRepositoryTests.cs
using Microsoft.EntityFrameworkCore;
using OrderService.Core.Entities;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories;

namespace OrderService.UnitTests;

public class EfCoreOrderRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly EfCoreOrderRepository _sut;

    public EfCoreOrderRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // DB isolato per ogni test
            .Options;

        _context = new AppDbContext(options);
        _sut     = new EfCoreOrderRepository(_context);
    }

    [Fact]
    public async Task AddAsync_PersistsOrder()
    {
        var order = new Order { Customer = "Alice", Total = 99m };

        var saved = await _sut.AddAsync(order);

        Assert.True(saved.Id > 0);
        Assert.Equal("Alice", saved.Customer);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingOrder_ReturnsIt()
    {
        var order = new Order { Customer = "Bob", Total = 50m };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var found = await _sut.GetByIdAsync(order.Id);

        Assert.NotNull(found);
        Assert.Equal("Bob", found.Customer);
    }

    [Fact]
    public async Task GetByIdAsync_MissingOrder_ReturnsNull()
    {
        var found = await _sut.GetByIdAsync(999);
        Assert.Null(found);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllOrders()
    {
        _context.Orders.AddRange(
            new Order { Customer = "Alice", Total = 10m },
            new Order { Customer = "Bob",   Total = 20m });
        await _context.SaveChangesAsync();

        var all = await _sut.GetAllAsync();

        Assert.Equal(2, all.Count());
    }

    public void Dispose() => _context.Dispose();
}
