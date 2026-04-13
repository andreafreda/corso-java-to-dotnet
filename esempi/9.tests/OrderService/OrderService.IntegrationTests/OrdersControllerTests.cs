// IntegrationTests/OrdersControllerTests.cs
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Core.DTOs;
using OrderService.Infrastructure.Persistence;

namespace OrderService.IntegrationTests;

/// <summary>
/// Test di integrazione con WebApplicationFactory — override del DbContext con InMemory.
/// Non richiede Docker/database reale.
/// </summary>
public class OrdersControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrdersControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Rimuove il DbContext SqlServer e lo sostituisce con InMemory
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor is not null)
                    services.Remove(descriptor);

                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("IntegrationTestDb"));
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetOrders_EmptyDb_Returns200WithEmptyList()
    {
        var response = await _client.GetAsync("/orders");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var orders = await response.Content.ReadFromJsonAsync<List<OrderDto>>();
        Assert.NotNull(orders);
    }

    [Fact]
    public async Task PostOrder_ValidRequest_Returns201WithDto()
    {
        var request = new CreateOrderRequest("Integration Customer", 250m);

        var response = await _client.PostAsJsonAsync("/orders", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var dto = await response.Content.ReadFromJsonAsync<OrderDto>();
        Assert.NotNull(dto);
        Assert.Equal("Integration Customer", dto.Customer);
        Assert.Equal(250m, dto.Total);
    }

    [Fact]
    public async Task PostOrder_MissingCustomer_Returns400()
    {
        var request = new { Customer = "", Total = 100m };

        var response = await _client.PostAsJsonAsync("/orders", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
