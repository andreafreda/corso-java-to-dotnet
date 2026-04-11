// Infrastructure/Http/InventoryClient.cs
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using OrderService.Core.Interfaces;

namespace OrderService.Infrastructure.Http;

public class InventoryClient : IInventoryClient
{
    private readonly HttpClient _client;
    private readonly ILogger<InventoryClient> _logger;

    // HttpClient viene iniettato già configurato da IHttpClientFactory — nessuna stringa magica
    public InventoryClient(HttpClient client, ILogger<InventoryClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<bool> IsAvailableAsync(int productId, int quantity, CancellationToken ct = default)
    {
        try
        {
            var response = await _client.GetAsync(
                $"/inventory/{productId}/available?quantity={quantity}", ct);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>(ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "InventoryService non raggiungibile per productId={ProductId}", productId);
            return false;  // fallback: assume non disponibile
        }
    }
}
