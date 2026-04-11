// Core/Entities/Order.cs
namespace OrderService.Core.Entities;

public class Order
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Customer { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Cancelled
}
