// Infrastructure/DependencyInjection.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Core.Interfaces;
using OrderService.Core.Interfaces.Repositories;
using OrderService.Infrastructure.Auth;
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

        // Singleton — il dictionary dei refresh token deve sopravvivere tra le richieste
        services.AddSingleton<IRefreshTokenStore, InMemoryRefreshTokenStore>();

        return services;
    }
}
