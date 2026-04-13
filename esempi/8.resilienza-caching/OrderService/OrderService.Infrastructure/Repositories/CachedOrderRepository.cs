// Infrastructure/Repositories/CachedOrderRepository.cs
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using OrderService.Core.Entities;
using OrderService.Core.Interfaces.Repositories;

namespace OrderService.Infrastructure.Repositories;

/// <summary>
/// Decorator di IOrderRepository: aggiunge caching Redis su GetByIdAsync.
/// Chiave: "order:{id}", TTL 5 minuti.
/// </summary>
public class CachedOrderRepository : IOrderRepository
{
    private readonly IOrderRepository      _inner;
    private readonly IDistributedCache     _cache;
    private readonly ILogger<CachedOrderRepository> _logger;

    private static readonly DistributedCacheEntryOptions CacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
    };

    public CachedOrderRepository(
        IOrderRepository inner,
        IDistributedCache cache,
        ILogger<CachedOrderRepository> logger)
    {
        _inner  = inner;
        _cache  = cache;
        _logger = logger;
    }

    public Task<IEnumerable<Order>> GetAllAsync(CancellationToken ct = default)
        => _inner.GetAllAsync(ct);

    public async Task<Order?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var key = $"order:{id}";

        var cached = await _cache.GetStringAsync(key, ct);
        if (cached is not null)
        {
            _logger.LogDebug("Cache HIT per order:{Id}", id);
            return JsonSerializer.Deserialize<Order>(cached);
        }

        var order = await _inner.GetByIdAsync(id, ct);
        if (order is not null)
        {
            await _cache.SetStringAsync(
                key,
                JsonSerializer.Serialize(order),
                CacheOptions,
                ct);
        }

        return order;
    }

    public Task<Order> AddAsync(Order order, CancellationToken ct = default)
        => _inner.AddAsync(order, ct);
}
