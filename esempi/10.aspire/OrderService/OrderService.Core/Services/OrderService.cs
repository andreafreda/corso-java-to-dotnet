// Core/Services/OrderService.cs
using OrderService.Core.DTOs;
using OrderService.Core.Entities;
using OrderService.Core.Interfaces.Repositories;
using OrderService.Core.Interfaces.Services;

namespace OrderService.Core.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync(CancellationToken ct = default)
    {
        var orders = await _repository.GetAllAsync(ct);
        return orders.Select(ToDto);
    }

    public async Task<OrderDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var order = await _repository.GetByIdAsync(id, ct);
        return order is null ? null : ToDto(order);
    }

    public async Task<OrderDto> CreateAsync(CreateOrderRequest request, CancellationToken ct = default)
    {
        var order = new Order
        {
            Customer = request.Customer,
            Total    = request.Total
        };
        var saved = await _repository.AddAsync(order, ct);
        return ToDto(saved);
    }

    private static OrderDto ToDto(Order o) =>
        new(o.Id, o.Customer, o.Total, o.Status.ToString(), o.CreatedAt);
}
