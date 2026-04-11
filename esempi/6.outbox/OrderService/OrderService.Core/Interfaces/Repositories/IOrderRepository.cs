// Core/Interfaces/Repositories/IOrderRepository.cs
using OrderService.Core.Entities;
using OrderService.Core.Events;

namespace OrderService.Core.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync(CancellationToken ct = default);
    Task<Order?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Order> AddAsync(Order order, CancellationToken ct = default);
    Task<Order> AddWithOutboxAsync(Order order, OrderCreatedEvent eventData, CancellationToken ct = default);
}
