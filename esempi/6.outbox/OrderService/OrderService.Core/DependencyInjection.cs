// Core/DependencyInjection.cs
using Microsoft.Extensions.DependencyInjection;
using OrderService.Core.Interfaces.Services;

namespace OrderService.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService.Core.Services.OrderService>();
        return services;
    }
}
