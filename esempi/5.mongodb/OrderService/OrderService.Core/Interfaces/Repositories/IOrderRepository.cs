// Core/Interfaces/Repositories/IOrderRepository.cs
using OrderService.Core.Entities;

namespace OrderService.Core.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync(CancellationToken ct = default);
    Task<Order?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<Order> AddAsync(Order order, CancellationToken ct = default);
}
