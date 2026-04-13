// Core/Services/OrderService.cs
using System.Diagnostics;
using OrderService.Core.DTOs;
using OrderService.Core.Entities;
using OrderService.Core.Interfaces.Repositories;
using OrderService.Core.Interfaces.Services;

namespace OrderService.Core.Services;

public class OrderService : IOrderService
{
    // ActivitySource condiviso — il nome corrisponde alla source registrata in Program.cs
    public static readonly ActivitySource ActivitySource = new("OrderService");

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
        using var activity = ActivitySource.StartActivity("CreateOrder");
        activity?.SetTag("order.customer", request.Customer);
        activity?.SetTag("order.total", request.Total);

        var order = new Order
        {
            Customer = request.Customer,
            Total    = request.Total
        };
        var saved = await _repository.AddAsync(order, ct);

        activity?.SetTag("order.id", saved.Id);
        return ToDto(saved);
    }

    private static OrderDto ToDto(Order o) =>
        new(o.Id, o.Customer, o.Total, o.Status.ToString(), o.CreatedAt);
}
