// Infrastructure/Repositories/MongoOrderRepository.cs
using MongoDB.Driver;
using OrderService.Core.Entities;
using OrderService.Core.Interfaces.Repositories;
using OrderService.Infrastructure.Documents;

namespace OrderService.Infrastructure.Repositories;

public class MongoOrderRepository : IOrderRepository
{
    private readonly IMongoCollection<OrderDocument> _collection;

    public MongoOrderRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<OrderDocument>("orders");
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken ct = default)
    {
        var docs = await _collection.Find(_ => true).ToListAsync(ct);
        return docs.Select(ToOrder);
    }

    public async Task<Order?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        var filter = Builders<OrderDocument>.Filter.Eq(o => o.Id, id);
        var doc = await _collection.Find(filter).FirstOrDefaultAsync(ct);
        return doc is null ? null : ToOrder(doc);
    }

    public async Task<Order> AddAsync(Order order, CancellationToken ct = default)
    {
        var doc = ToDocument(order);
        await _collection.InsertOneAsync(doc, cancellationToken: ct);
        // Id è popolato da MongoDB dopo InsertOneAsync
        return ToOrder(doc);
    }

    private static OrderDocument ToDocument(Order o) => new()
    {
        Customer  = o.Customer,
        Total     = o.Total,
        Status    = o.Status.ToString(),
        CreatedAt = o.CreatedAt
    };

    private static Order ToOrder(OrderDocument d) => new()
    {
        Id        = d.Id!,
        Customer  = d.Customer,
        Total     = d.Total,
        Status    = Enum.Parse<OrderStatus>(d.Status),
        CreatedAt = d.CreatedAt
    };
}
