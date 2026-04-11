// Infrastructure/DependencyInjection.cs
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Core.Interfaces;
using OrderService.Core.Interfaces.Repositories;
using OrderService.Infrastructure.Messaging;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories;

namespace OrderService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        services.AddScoped<IOrderRepository, EfCoreOrderRepository>();

        services.AddSingleton(new ServiceBusClient(
            configuration.GetConnectionString("ServiceBus")));
        services.AddScoped<IOrderEventPublisher, OrderEventPublisher>();
        services.AddHostedService<OrderCreatedConsumer>();

        return services;
    }
}
