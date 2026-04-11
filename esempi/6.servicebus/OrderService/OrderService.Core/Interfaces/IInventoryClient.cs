// Core/Interfaces/IInventoryClient.cs
namespace OrderService.Core.Interfaces;

public interface IInventoryClient
{
    Task<bool> IsAvailableAsync(int productId, int quantity, CancellationToken ct = default);
}
