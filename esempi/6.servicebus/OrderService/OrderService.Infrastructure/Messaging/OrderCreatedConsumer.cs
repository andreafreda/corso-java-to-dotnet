// Infrastructure/Messaging/OrderCreatedConsumer.cs
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderService.Core.Events;

namespace OrderService.Infrastructure.Messaging;

public class OrderCreatedConsumer : BackgroundService
{
    private readonly ServiceBusProcessor _processor;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OrderCreatedConsumer> _logger;

    public OrderCreatedConsumer(
        ServiceBusClient client,
        IServiceScopeFactory scopeFactory,
        ILogger<OrderCreatedConsumer> logger)
    {
        _processor    = client.CreateProcessor("order-created", "inventory-sub");
        _scopeFactory = scopeFactory;
        _logger       = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        _processor.ProcessMessageAsync += HandleMessageAsync;
        _processor.ProcessErrorAsync   += HandleErrorAsync;

        await _processor.StartProcessingAsync(ct);

        // Resta in ascolto fino alla cancellazione
        await Task.Delay(Timeout.Infinite, ct).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);

        await _processor.StopProcessingAsync();
    }

    private async Task HandleMessageAsync(ProcessMessageEventArgs args)
    {
        var body = args.Message.Body.ToString();
        var evt  = JsonSerializer.Deserialize<OrderCreatedEvent>(body);

        if (evt is null)
        {
            await args.DeadLetterMessageAsync(args.Message, "DeserializationFailed", "Body non valido");
            return;
        }

        _logger.LogInformation("Elaborazione OrderCreated per OrderId={OrderId}", evt.OrderId);

        // IServiceScopeFactory necessario perché BackgroundService è Singleton
        // ma i servizi di dominio sono Scoped
        using var scope = _scopeFactory.CreateScope();

        // In un microservizio reale risolveresti qui il tuo servizio di dominio, es.:
        // var inventoryService = scope.ServiceProvider.GetRequiredService<IInventoryService>();
        // await inventoryService.DecrementStockAsync(evt.OrderId, args.CancellationToken);
        //
        // Questo consumer è nel progetto OrderService solo a scopo dimostrativo —
        // in produzione, InventoryService avrebbe il proprio consumer con la propria logica.
        await Task.CompletedTask;

        await args.CompleteMessageAsync(args.Message);
    }

    private Task HandleErrorAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception,
            "Errore nel processing Service Bus: {ErrorSource}", args.ErrorSource);
        return Task.CompletedTask;
    }
}
