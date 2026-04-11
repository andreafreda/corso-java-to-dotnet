using FluentValidation;
using Microsoft.Azure.Cosmos;
using OrderService.Api.Filters;
using OrderService.Api.Middleware;
using OrderService.Api.Validators;
using OrderService.Core;
using OrderService.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
    options.Filters.Add<ValidationFilter>());
builder.Services.AddOpenApi();

builder.Services.AddCore();
builder.Services.AddInfrastructure();

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var endpoint = builder.Configuration["CosmosDb:Endpoint"]!;
var key      = builder.Configuration["CosmosDb:Key"]!;

// Il bypass SSL è necessario solo con l'emulatore locale — mai in produzione
var cosmosOptions = builder.Environment.IsDevelopment()
    ? new CosmosClientOptions
      {
          HttpClientFactory = () => new HttpClient(new HttpClientHandler
          {
              ServerCertificateCustomValidationCallback =
                  HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
          }),
          ConnectionMode = ConnectionMode.Gateway
      }
    : new CosmosClientOptions();

builder.Services.AddSingleton(new CosmosClient(endpoint, key, cosmosOptions));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Crea database e container se non esistono
    var cosmos = app.Services.GetRequiredService<CosmosClient>();
    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
    var db = await cosmos.CreateDatabaseIfNotExistsAsync("OrderDb", cancellationToken: cts.Token);
    await db.Database.CreateContainerIfNotExistsAsync("orders", "/partitionKey");

    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.MapControllers();
app.Run();
