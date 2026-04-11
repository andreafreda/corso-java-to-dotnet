// Infrastructure/Repositories/CosmosOrderRepository.cs
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using OrderService.Core.Entities;
using OrderService.Core.Interfaces.Repositories;
using OrderService.Infrastructure.Documents;

namespace OrderService.Infrastructure.Repositories;

public class CosmosOrderRepository : IOrderRepository
{
    private readonly Container _container;

    public CosmosOrderRepository(CosmosClient client)
    {
        _container = client.GetContainer("OrderDb", "orders");
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken ct = default)
    {
        var iterator = _container
            .GetItemLinqQueryable<OrderDocument>()
            .ToFeedIterator();

        var results = new List<OrderDocument>();
        while (iterator.HasMoreResults)
        {
            var page = await iterator.ReadNextAsync(ct);
            results.AddRange(page);
        }
        return results.Select(ToOrder);
    }

    public async Task<Order?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        // Senza partition key nota, eseguiamo una query cross-partition
        var iterator = _container
            .GetItemLinqQueryable<OrderDocument>(requestOptions: new QueryRequestOptions { MaxItemCount = 1 })
            .Where(o => o.Id == id)
            .ToFeedIterator();

        while (iterator.HasMoreResults)
        {
            var page = await iterator.ReadNextAsync(ct);
            var doc = page.FirstOrDefault();
            if (doc is not null) return ToOrder(doc);
        }
        return null;
    }

    public async Task<Order> AddAsync(Order order, CancellationToken ct = default)
    {
        var doc = ToDocument(order);
        var response = await _container.CreateItemAsync(
            doc,
            new PartitionKey(doc.PartitionKey),
            cancellationToken: ct);
        return ToOrder(response.Resource);
    }

    private static OrderDocument ToDocument(Order o) => new()
    {
        Id         = o.Id,
        Customer   = o.Customer,
        Total      = o.Total,
        Status     = o.Status.ToString(),
        CreatedAt  = o.CreatedAt
    };

    private static Order ToOrder(OrderDocument d) => new()
    {
        Id        = d.Id,
        Customer  = d.Customer,
        Total     = d.Total,
        Status    = Enum.Parse<OrderStatus>(d.Status),
        CreatedAt = d.CreatedAt
    };
}
