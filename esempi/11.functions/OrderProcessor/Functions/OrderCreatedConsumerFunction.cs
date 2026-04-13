// Functions/OrderCreatedConsumerFunction.cs
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace OrderProcessor.Functions;

public record OrderCreatedEvent(int OrderId, string Customer, decimal Total, DateTime CreatedAt);

public class OrderCreatedConsumerFunction
{
    private readonly ILogger<OrderCreatedConsumerFunction> _logger;

    public OrderCreatedConsumerFunction(ILogger<OrderCreatedConsumerFunction> logger)
    {
        _logger = logger;
    }

    // ServiceBusTrigger: subscription "inventory-sub" sul topic "order-created"
    [Function("OrderCreatedConsumer")]
    public void Run(
        [ServiceBusTrigger("order-created", "inventory-sub",
            Connection = "ServiceBusConnection")] string messageBody)
    {
        var evt = JsonSerializer.Deserialize<OrderCreatedEvent>(messageBody);

        _logger.LogInformation(
            "Ricevuto OrderCreatedEvent — OrderId={OrderId}, Customer={Customer}, Total={Total}",
            evt?.OrderId, evt?.Customer, evt?.Total);

        // Logica inventario (stub per il corso)
    }
}
