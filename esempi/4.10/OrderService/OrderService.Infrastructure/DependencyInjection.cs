// Infrastructure/DependencyInjection.cs
using Microsoft.Extensions.DependencyInjection;
using OrderService.Core.Interfaces.Repositories;
using OrderService.Infrastructure.Repositories;

namespace OrderService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Nel capitolo 5 questa riga diventa: services.AddDbContext + AddScoped<IOrderRepository, OrderRepository>
        services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        return services;
    }
}
