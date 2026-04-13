// Core/Interfaces/IRefreshTokenStore.cs
using OrderService.Core.Entities;

namespace OrderService.Core.Interfaces;

public interface IRefreshTokenStore
{
    void          Store(RefreshToken token);
    RefreshToken? Get(string token);
    void          Revoke(string token);
}
