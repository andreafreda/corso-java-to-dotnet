// Core/Events/OrderCreatedEvent.cs
namespace OrderService.Core.Events;

public record OrderCreatedEvent(
    int     OrderId,
    string  Customer,
    decimal Total,
    DateTime CreatedAt);
