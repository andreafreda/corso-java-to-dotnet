// Infrastructure/Repositories/EfCoreOrderRepository.cs
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OrderService.Core.Entities;
using OrderService.Core.Events;
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

    public async Task<Order> AddWithOutboxAsync(
        Order order, OrderCreatedEvent eventData, CancellationToken ct = default)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        _db.Orders.Add(order);
        await _db.SaveChangesAsync(ct);  // order.Id ora disponibile (generato dal DB)

        // Serializzazione in Infrastructure — il Core non tocca JsonSerializer
        _db.Outbox.Add(new OutboxMessage
        {
            Type    = "OrderCreated",
            Payload = JsonSerializer.Serialize(eventData with { OrderId = order.Id })
        });
        await _db.SaveChangesAsync(ct);

        await tx.CommitAsync(ct);  // unico commit atomico

        return order;
    }
}
