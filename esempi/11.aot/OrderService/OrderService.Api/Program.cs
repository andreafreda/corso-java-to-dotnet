// Native AOT — niente reflection, niente JIT
// Differenze chiave rispetto a 5.ef-azuresql:
//   1. CreateSlimBuilder (non CreateBuilder)
//   2. Minimal API (non Controllers — i controller usano reflection)
//   3. [JsonSerializable] source generator per la serializzazione JSON
//   4. FluentValidation rimossa (reflection-heavy)
//   5. Scalar rimosso (non necessario per AOT)

using System.Text.Json.Serialization;
using OrderService.Api.Middleware;
using OrderService.Core;
using OrderService.Core.DTOs;
using OrderService.Core.Interfaces.Services;
using OrderService.Infrastructure;

var builder = WebApplication.CreateSlimBuilder(args);

// Configura System.Text.Json con il source generator — richiesto per AOT
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default));

builder.Services.AddCore();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

// ── Minimal API endpoints ────────────────────────────────────────────────────

var orders = app.MapGroup("/orders");

orders.MapGet("/", async (IOrderService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetAllAsync(ct)));

orders.MapGet("/{id:int}", async (int id, IOrderService svc, CancellationToken ct) =>
{
    var dto = await svc.GetByIdAsync(id, ct);
    return dto is null ? Results.NotFound() : Results.Ok(dto);
});

orders.MapPost("/", async (CreateOrderRequest request, IOrderService svc, CancellationToken ct) =>
{
    // Validazione manuale — FluentValidation usa reflection (non AOT-compatibile)
    if (string.IsNullOrWhiteSpace(request.Customer))
        return Results.Problem("Customer is required", statusCode: 400);
    if (request.Total <= 0)
        return Results.Problem("Total must be positive", statusCode: 400);

    var dto = await svc.CreateAsync(request, ct);
    return Results.Created($"/orders/{dto.Id}", dto);
});

app.Run();

// JSON Source Generator — [JsonSerializable] indica al compilatore quali tipi serializzare.
// Necessario perché AOT non può usare reflection a runtime.
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(OrderDto))]
[JsonSerializable(typeof(List<OrderDto>))]
[JsonSerializable(typeof(CreateOrderRequest))]
[JsonSerializable(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails))]
internal partial class AppJsonSerializerContext : JsonSerializerContext { }
