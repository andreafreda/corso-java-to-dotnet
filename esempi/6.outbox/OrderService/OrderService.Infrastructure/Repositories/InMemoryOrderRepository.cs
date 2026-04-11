// Infrastructure/Repositories/InMemoryOrderRepository.cs
using OrderService.Core.Entities;
using OrderService.Core.Events;
using OrderService.Core.Interfaces.Repositories;

namespace OrderService.Infrastructure.Repositories;

// Implementazione temporanea — verrà sostituita con EF Core nel capitolo 5
public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _store =
    [
        new() { Id = 1, Customer = "Mario Rossi",    Total = 150.00m, Status = OrderStatus.Confirmed },
        new() { Id = 2, Customer = "Giulia Bianchi", Total =  89.50m, Status = OrderStatus.Pending   },
    ];

    private int _nextId = 3;

    public Task<IEnumerable<Order>> GetAllAsync(CancellationToken ct = default) =>
        Task.FromResult<IEnumerable<Order>>(_store);

    public Task<Order?> GetByIdAsync(int id, CancellationToken ct = default) =>
        Task.FromResult(_store.FirstOrDefault(o => o.Id == id));

    public Task<Order> AddAsync(Order order, CancellationToken ct = default)
    {
        order.Id = _nextId++;
        _store.Add(order);
        return Task.FromResult(order);
    }

    // In-memory: nessuna transazione reale — comportamento equivalente senza Outbox
    public Task<Order> AddWithOutboxAsync(Order order, OrderCreatedEvent eventData, CancellationToken ct = default)
        => AddAsync(order, ct);
}
