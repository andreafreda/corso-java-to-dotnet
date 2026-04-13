// Infrastructure/Auth/InMemoryRefreshTokenStore.cs
using System.Collections.Concurrent;
using OrderService.Core.Entities;
using OrderService.Core.Interfaces;

namespace OrderService.Infrastructure.Auth;

// In produzione: sostituisci con un'implementazione che legge/scrive su DB
public class InMemoryRefreshTokenStore : IRefreshTokenStore
{
    // ConcurrentDictionary — thread-safe per richieste concorrenti
    private readonly ConcurrentDictionary<string, RefreshToken> _store = new();

    public void Store(RefreshToken token)
        => _store[token.Token] = token;

    public RefreshToken? Get(string token)
        => _store.GetValueOrDefault(token);

    public void Revoke(string token)
    {
        if (_store.TryGetValue(token, out var existing))
            existing.Revoked = true;
    }
}
