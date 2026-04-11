// Infrastructure/Repositories/InMemoryOrderRepository.cs
using OrderService.Core.Entities;
using OrderService.Core.Interfaces.Repositories;

namespace OrderService.Infrastructure.Repositories;

// Mantenuto per i test unitari — non registrato nel DI in questo esempio
public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _store =
    [
        new() { Id = Guid.NewGuid().ToString(), Customer = "Mario Rossi",    Total = 150.00m, Status = OrderStatus.Confirmed },
        new() { Id = Guid.NewGuid().ToString(), Customer = "Giulia Bianchi", Total =  89.50m, Status = OrderStatus.Pending   },
    ];

    public Task<IEnumerable<Order>> GetAllAsync(CancellationToken ct = default) =>
        Task.FromResult<IEnumerable<Order>>(_store);

    public Task<Order?> GetByIdAsync(string id, CancellationToken ct = default) =>
        Task.FromResult(_store.FirstOrDefault(o => o.Id == id));

    public Task<Order> AddAsync(Order order, CancellationToken ct = default)
    {
        _store.Add(order);
        return Task.FromResult(order);
    }
}
