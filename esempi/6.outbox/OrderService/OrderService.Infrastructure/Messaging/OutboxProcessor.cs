// Infrastructure/Messaging/OutboxProcessor.cs
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Messaging;

public class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory    _scopeFactory;
    private readonly ServiceBusSender        _sender;
    private readonly ILogger<OutboxProcessor> _logger;

    public OutboxProcessor(
        IServiceScopeFactory scopeFactory,
        ServiceBusClient serviceBusClient,
        ILogger<OutboxProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _sender       = serviceBusClient.CreateSender("order-created");
        _logger       = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await ProcessPendingMessagesAsync(ct);
            await Task.Delay(TimeSpan.FromSeconds(5), ct).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
        }
    }

    private async Task ProcessPendingMessagesAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var db          = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var pending = await db.Outbox
            .Where(o => o.ProcessedAt == null)
            .OrderBy(o => o.CreatedAt)
            .Take(10)
            .ToListAsync(ct);

        if (pending.Count == 0) return;

        foreach (var outbox in pending)
        {
            try
            {
                var message = new ServiceBusMessage(outbox.Payload)
                {
                    ContentType = "application/json",
                    Subject     = outbox.Type,
                    MessageId   = outbox.Id.ToString()
                };

                await _sender.SendMessageAsync(message, ct);

                outbox.ProcessedAt = DateTime.UtcNow;
                _logger.LogInformation(
                    "Outbox message {Id} ({Type}) pubblicato su Service Bus", outbox.Id, outbox.Type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore pubblicazione outbox message {Id}", outbox.Id);
                // non marcare come processed — verrà riprovato al prossimo ciclo
            }
        }

        await db.SaveChangesAsync(ct);
    }
}
