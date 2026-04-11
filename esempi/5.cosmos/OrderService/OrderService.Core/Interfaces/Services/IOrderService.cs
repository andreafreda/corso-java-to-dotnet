// Core/Interfaces/Services/IOrderService.cs
using OrderService.Core.DTOs;

namespace OrderService.Core.Interfaces.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllAsync(CancellationToken ct = default);
    Task<OrderDto?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<OrderDto> CreateAsync(CreateOrderRequest request, CancellationToken ct = default);
}
