// Functions/ProcessOrderFunction.cs
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace OrderProcessor.Functions;

public class ProcessOrderFunction
{
    private readonly ILogger<ProcessOrderFunction> _logger;

    public ProcessOrderFunction(ILogger<ProcessOrderFunction> logger)
    {
        _logger = logger;
    }

    // HttpTrigger: POST /orders/{id}/process
    [Function("ProcessOrder")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post",
            Route = "orders/{id}/process")] HttpRequestData req,
        int id)
    {
        _logger.LogInformation("Processing order {OrderId}", id);

        string body = await new StreamReader(req.Body).ReadToEndAsync();

        // Logica di elaborazione (stub per il corso)
        var result = new { OrderId = id, Status = "Processed", ProcessedAt = DateTime.UtcNow };

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(result);
        return response;
    }
}
