// Infrastructure/Messaging/OrderEventPublisher.cs
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using OrderService.Core.Events;
using OrderService.Core.Interfaces;

namespace OrderService.Infrastructure.Messaging;

public class OrderEventPublisher : IOrderEventPublisher
{
    private readonly ServiceBusSender _sender;
    private readonly ILogger<OrderEventPublisher> _logger;

    public OrderEventPublisher(ServiceBusClient client, ILogger<OrderEventPublisher> logger)
    {
        _sender = client.CreateSender("order-created");
        _logger = logger;
    }

    public async Task PublishOrderCreatedAsync(OrderCreatedEvent evt, CancellationToken ct = default)
    {
        var body    = JsonSerializer.Serialize(evt);
        var message = new ServiceBusMessage(body)
        {
            ContentType = "application/json",
            Subject     = "OrderCreated",
            MessageId   = $"order-{evt.OrderId}-{Guid.NewGuid()}"
        };

        await _sender.SendMessageAsync(message, ct);
        _logger.LogInformation("Evento OrderCreated pubblicato per OrderId={OrderId}", evt.OrderId);
    }
}
