// Infrastructure/DependencyInjection.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using OrderService.Core.Interfaces;
using OrderService.Core.Interfaces.Repositories;
using OrderService.Infrastructure.Http;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories;
using Polly;

namespace OrderService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        // Redis IDistributedCache
        services.AddStackExchangeRedisCache(options =>
            options.Configuration = configuration.GetConnectionString("Redis"));

        // Decorator: IOrderRepository → CachedOrderRepository(EfCoreOrderRepository)
        services.AddScoped<EfCoreOrderRepository>();
        services.AddScoped<IOrderRepository>(sp =>
            new CachedOrderRepository(
                sp.GetRequiredService<EfCoreOrderRepository>(),
                sp.GetRequiredService<Microsoft.Extensions.Caching.Distributed.IDistributedCache>(),
                sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<CachedOrderRepository>>()));

        // Typed client con Polly: retry esponenziale + circuit breaker
        services.AddHttpClient<IInventoryClient, InventoryClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["Services:InventoryUrl"]!);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddResilienceHandler("inventory-pipeline", builder =>
            {
                // Retry: 3 tentativi con backoff esponenziale
                builder.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    BackoffType      = DelayBackoffType.Exponential,
                    Delay            = TimeSpan.FromMilliseconds(200)
                });

                // Circuit Breaker: si apre dopo 5 errori su 10 richieste per 30 secondi
                builder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                {
                    FailureRatio            = 0.5,   // 50% failure rate
                    MinimumThroughput       = 5,
                    BreakDuration           = TimeSpan.FromSeconds(30),
                    SamplingDuration        = TimeSpan.FromSeconds(10)
                });
            });

        return services;
    }
}
