// Core/Interfaces/IOrderEventPublisher.cs
using OrderService.Core.Events;

namespace OrderService.Core.Interfaces;

public interface IOrderEventPublisher
{
    Task PublishOrderCreatedAsync(OrderCreatedEvent evt, CancellationToken ct = default);
}
