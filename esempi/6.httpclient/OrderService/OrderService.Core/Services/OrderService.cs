// Core/Services/OrderService.cs
using OrderService.Core.DTOs;
using OrderService.Core.Entities;
using OrderService.Core.Interfaces;
using OrderService.Core.Interfaces.Repositories;
using OrderService.Core.Interfaces.Services;

namespace OrderService.Core.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IInventoryClient _inventoryClient;

    public OrderService(IOrderRepository repository, IInventoryClient inventoryClient)
    {
        _repository      = repository;
        _inventoryClient = inventoryClient;
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
        var available = await _inventoryClient.IsAvailableAsync(request.ProductId, request.Quantity, ct);
        if (!available)
            throw new InvalidOperationException($"Prodotto {request.ProductId} non disponibile");

        var order = new Order
        {
            Customer  = request.Customer,
            Total     = request.Total,
            Status    = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var saved = await _repository.AddAsync(order, ct);
        return ToDto(saved);
    }

    private static OrderDto ToDto(Order o) =>
        new(o.Id, o.Customer, o.Total, o.Status.ToString(), o.CreatedAt);
}
