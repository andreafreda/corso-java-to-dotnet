// Infrastructure/Documents/OrderDocument.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderService.Infrastructure.Documents;

public class OrderDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("customer")]
    public string Customer { get; set; } = string.Empty;

    [BsonElement("total")]
    public decimal Total { get; set; }

    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
