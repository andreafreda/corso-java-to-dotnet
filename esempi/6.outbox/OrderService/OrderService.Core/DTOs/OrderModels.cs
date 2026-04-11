// Core/DTOs/OrderModels.cs
namespace OrderService.Core.DTOs;

public record OrderDto(int Id, string Customer, decimal Total, string Status, DateTime CreatedAt);
public record CreateOrderRequest(string Customer, decimal Total);
