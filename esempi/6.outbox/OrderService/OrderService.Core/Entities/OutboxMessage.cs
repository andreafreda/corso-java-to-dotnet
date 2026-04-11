// Core/Entities/OutboxMessage.cs
namespace OrderService.Core.Entities;

public class OutboxMessage
{
    public Guid      Id          { get; set; } = Guid.NewGuid();
    public string    Type        { get; set; } = string.Empty;  // es. "OrderCreated"
    public string    Payload     { get; set; } = string.Empty;  // JSON dell'evento
    public DateTime  CreatedAt   { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }                  // null = non ancora pubblicato
}
