// Core/Interfaces/Services/IOrderService.cs — nuovo
using OrderService.Core.DTOs;

namespace OrderService.Core.Interfaces.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllAsync(CancellationToken ct = default);
    Task<OrderDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<OrderDto> CreateAsync(CreateOrderRequest request, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
