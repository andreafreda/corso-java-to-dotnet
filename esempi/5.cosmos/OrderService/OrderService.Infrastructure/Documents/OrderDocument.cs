// Infrastructure/Documents/OrderDocument.cs
// Il Cosmos SDK 3.x usa Newtonsoft.Json come serializer di default —
// vanno usati [JsonProperty] di Newtonsoft, non [JsonPropertyName] di System.Text.Json
using Newtonsoft.Json;

namespace OrderService.Infrastructure.Documents;

public class OrderDocument
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("customer")]
    public string Customer { get; set; } = string.Empty;

    [JsonProperty("total")]
    public decimal Total { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Partition key — distribuisce i dati tra le partizioni fisiche
    // "status" è una scelta didattica; in produzione usa un campo ad alta cardinalità
    [JsonProperty("partitionKey")]
    public string PartitionKey => Status;
}
