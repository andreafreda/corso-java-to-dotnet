// Infrastructure/Repositories/EfCoreOrderRepository.cs
using Microsoft.EntityFrameworkCore;
using OrderService.Core.Entities;
using OrderService.Core.Interfaces.Repositories;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories;

public class EfCoreOrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;

    public EfCoreOrderRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Orders.ToListAsync(ct);

    public async Task<Order?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _db.Orders.FindAsync([id], ct);

    public async Task<Order> AddAsync(Order order, CancellationToken ct = default)
    {
        _db.Orders.Add(order);
        await _db.SaveChangesAsync(ct);
        return order;
    }
}
