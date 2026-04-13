// Infrastructure/HealthChecks/OrderServiceHealthCheck.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.HealthChecks;

public class OrderServiceHealthCheck : IHealthCheck
{
    private readonly AppDbContext _db;

    public OrderServiceHealthCheck(AppDbContext db)
    {
        _db = db;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Query leggera: conta solo le righe senza caricare dati
            var count = await _db.Orders.CountAsync(cancellationToken);
            return HealthCheckResult.Healthy($"Orders in database: {count}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Cannot query orders table", ex);
        }
    }
}
