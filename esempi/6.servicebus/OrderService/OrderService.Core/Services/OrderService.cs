// Core/Services/OrderService.cs
using OrderService.Core.DTOs;
using OrderService.Core.Entities;
using OrderService.Core.Events;
using OrderService.Core.Interfaces;
using OrderService.Core.Interfaces.Repositories;
using OrderService.Core.Interfaces.Services;

namespace OrderService.Core.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository     _repository;
    private readonly IOrderEventPublisher _publisher;

    public OrderService(IOrderRepository repository, IOrderEventPublisher publisher)
    {
        _repository = repository;
        _publisher  = publisher;
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

        await _repository.AddAsync(order, ct);

        await _publisher.PublishOrderCreatedAsync(
            new OrderCreatedEvent(order.Id, order.Customer, order.Total, order.CreatedAt), ct);

        return ToDto(order);
    }

    private static OrderDto ToDto(Order o) =>
        new(o.Id, o.Customer, o.Total, o.Status.ToString(), o.CreatedAt);
}
