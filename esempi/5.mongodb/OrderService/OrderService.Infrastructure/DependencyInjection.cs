// Infrastructure/DependencyInjection.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OrderService.Core.Interfaces.Repositories;
using OrderService.Infrastructure.Repositories;

namespace OrderService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var mongoSettings = configuration.GetSection("MongoDb");

        services.AddSingleton<IMongoClient>(
            new MongoClient(mongoSettings["ConnectionString"]));

        services.AddScoped<IMongoDatabase>(sp =>
            sp.GetRequiredService<IMongoClient>().GetDatabase(mongoSettings["Database"]));

        services.AddScoped<IOrderRepository, MongoOrderRepository>();
        return services;
    }
}
