// IntegrationTests/OrdersControllerDbTests.cs
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Core.DTOs;
using OrderService.Infrastructure.Persistence;
using Testcontainers.MsSql;

namespace OrderService.IntegrationTests;

/// <summary>
/// Test di integrazione con Testcontainers — avvia SQL Server in Docker e applica le migrazioni.
/// Richiede Docker Desktop attivo. Più lento ma testa l'interazione reale con il database.
/// </summary>
public class OrdersControllerDbTests : IAsyncLifetime
{
    private readonly MsSqlContainer _sqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/azure-sql-edge:latest")
        .Build();

    private WebApplicationFactory<Program>? _factory;
    private HttpClient? _client;

    public async Task InitializeAsync()
    {
        await _sqlContainer.StartAsync();

        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor is not null)
                    services.Remove(descriptor);

                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(_sqlContainer.GetConnectionString()));
            });
        });

        _client = _factory.CreateClient();

        // Applica le migrazioni sul container
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        _factory?.Dispose();
        await _sqlContainer.StopAsync();
    }

    [Fact]
    public async Task PostOrder_RealDb_PersistsAndReturns201()
    {
        var request = new CreateOrderRequest("Real DB Customer", 500m);

        var response = await _client!.PostAsJsonAsync("/orders", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var dto = await response.Content.ReadFromJsonAsync<OrderDto>();
        Assert.NotNull(dto);
        Assert.True(dto.Id > 0);
        Assert.Equal("Real DB Customer", dto.Customer);
    }

    [Fact]
    public async Task GetById_AfterCreate_ReturnsOrder()
    {
        var created = await _client!.PostAsJsonAsync(
            "/orders", new CreateOrderRequest("Fetch Me", 99m));
        var dto = await created.Content.ReadFromJsonAsync<OrderDto>();

        Assert.NotNull(dto);
        var response = await _client!.GetAsync($"/orders/{dto.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var found = await response.Content.ReadFromJsonAsync<OrderDto>();
        Assert.Equal("Fetch Me", found!.Customer);
    }
}
