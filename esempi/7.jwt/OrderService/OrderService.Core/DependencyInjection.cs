// Core/DependencyInjection.cs
using Microsoft.Extensions.DependencyInjection;
using OrderService.Core.Interfaces.Services;
using OrderService.Core.Services;

namespace OrderService.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService.Core.Services.OrderService>();
        services.AddScoped<TokenService>();
        services.AddScoped<AuthService>();
        return services;
    }
}
