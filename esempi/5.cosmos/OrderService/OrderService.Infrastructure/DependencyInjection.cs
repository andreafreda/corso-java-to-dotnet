// Infrastructure/DependencyInjection.cs
using Microsoft.Extensions.DependencyInjection;
using OrderService.Core.Interfaces.Repositories;
using OrderService.Infrastructure.Repositories;

namespace OrderService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, CosmosOrderRepository>();
        return services;
    }
}
