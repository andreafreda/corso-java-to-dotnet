// NUnitTests/EfCoreOrderRepositoryTests.cs
using Microsoft.EntityFrameworkCore;
using OrderService.Core.Entities;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories;

namespace OrderService.NUnitTests;

[TestFixture]
public class EfCoreOrderRepositoryTests
{
    private AppDbContext _context = null!;
    private EfCoreOrderRepository _sut = null!;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // DB isolato per ogni test
            .Options;

        _context = new AppDbContext(options);
        _sut     = new EfCoreOrderRepository(_context);
    }

    [TearDown]
    public void TearDown() => _context.Dispose();

    [Test]
    public async Task AddAsync_PersistsOrder()
    {
        var order = new Order { Customer = "Alice", Total = 99m };

        var saved = await _sut.AddAsync(order);

        Assert.That(saved.Id, Is.GreaterThan(0));
        Assert.That(saved.Customer, Is.EqualTo("Alice"));
    }

    [Test]
    public async Task GetByIdAsync_ExistingOrder_ReturnsIt()
    {
        var order = new Order { Customer = "Bob", Total = 50m };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var found = await _sut.GetByIdAsync(order.Id);

        Assert.That(found, Is.Not.Null);
        Assert.That(found!.Customer, Is.EqualTo("Bob"));
    }

    [Test]
    public async Task GetByIdAsync_MissingOrder_ReturnsNull()
    {
        var found = await _sut.GetByIdAsync(999);
        Assert.That(found, Is.Null);
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllOrders()
    {
        _context.Orders.AddRange(
            new Order { Customer = "Alice", Total = 10m },
            new Order { Customer = "Bob",   Total = 20m });
        await _context.SaveChangesAsync();

        var all = await _sut.GetAllAsync();

        Assert.That(all.Count(), Is.EqualTo(2));
    }
}
