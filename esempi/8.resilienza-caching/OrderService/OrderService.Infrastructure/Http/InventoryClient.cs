// Infrastructure/Http/InventoryClient.cs
using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using OrderService.Core.Interfaces;

namespace OrderService.Infrastructure.Http;

public class InventoryClient : IInventoryClient
{
    private readonly HttpClient _client;
    private readonly IDistributedCache _cache;
    private readonly ILogger<InventoryClient> _logger;

    private static readonly DistributedCacheEntryOptions CacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
    };

    public InventoryClient(HttpClient client, IDistributedCache cache, ILogger<InventoryClient> logger)
    {
        _client = client;
        _cache  = cache;
        _logger = logger;
    }

    public async Task<bool> IsAvailableAsync(int productId, int quantity, CancellationToken ct = default)
    {
        var key = $"inventory:{productId}:{quantity}";

        var cached = await _cache.GetStringAsync(key, ct);
        if (cached is not null)
        {
            _logger.LogDebug("Cache HIT per inventory:{ProductId}:{Quantity}", productId, quantity);
            return bool.Parse(cached);
        }

        try
        {
            var response = await _client.GetAsync(
                $"/inventory/{productId}/available?quantity={quantity}", ct);

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<bool>(ct);

            await _cache.SetStringAsync(
                key,
                result.ToString(),
                CacheOptions,
                ct);

            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "InventoryService non raggiungibile per productId={ProductId}", productId);
            return false;  // fallback: assume non disponibile
        }
    }
}
