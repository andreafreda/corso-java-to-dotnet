// Core/Services/OrderService.cs
using OrderService.Core.DTOs;
using OrderService.Core.Entities;
using OrderService.Core.Events;
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
            Customer  = request.Customer,
            Total     = request.Total,
            Status    = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        // Il Core costruisce l'evento ma NON lo serializza — è responsabilità di Infrastructure
        var eventData = new OrderCreatedEvent(
            OrderId:  0,                  // Id non ancora assegnato — il repository lo sostituirà
            Customer: order.Customer,
            Total:    order.Total,
            CreatedAt: order.CreatedAt);

        await _repository.AddWithOutboxAsync(order, eventData, ct);

        return ToDto(order);
    }

    private static OrderDto ToDto(Order o) =>
        new(o.Id, o.Customer, o.Total, o.Status.ToString(), o.CreatedAt);
}
