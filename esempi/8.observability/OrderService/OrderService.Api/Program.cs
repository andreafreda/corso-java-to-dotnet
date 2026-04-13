using FluentValidation;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OrderService.Api.Filters;
using OrderService.Api.Middleware;
using OrderService.Api.Validators;
using OrderService.Core;
using OrderService.Core.Services;
using OrderService.Infrastructure;
using OrderService.Infrastructure.HealthChecks;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Formatting.Compact;

// Configura Serilog prima di tutto il resto
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new CompactJsonFormatter())
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Serilog
    builder.Host.UseSerilog((ctx, services, config) => config
        .ReadFrom.Configuration(ctx.Configuration)
        .ReadFrom.Services(services)
        .WriteTo.Console(new CompactJsonFormatter())
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName());

    builder.Services.AddControllers(options =>
        options.Filters.Add<ValidationFilter>());
    builder.Services.AddOpenApi();

    builder.Services.AddCore();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    // Health Checks
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<OrderService.Infrastructure.Persistence.AppDbContext>(
            name: "database",
            tags: ["ready"])
        .AddCheck<OrderServiceHealthCheck>(
            name: "orders-query",
            tags: ["ready"]);

    // OpenTelemetry
    builder.Services.AddOpenTelemetry()
        .ConfigureResource(r => r.AddService("OrderService"))
        .WithTracing(tracing => tracing
            .AddSource(global::OrderService.Core.Services.OrderService.ActivitySource.Name)
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter(o =>
                o.Endpoint = new Uri(
                    builder.Configuration["OpenTelemetry:OtlpEndpoint"] ?? "http://localhost:4317")));

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.UseSerilogRequestLogging();
    app.UseExceptionHandler();
    app.MapControllers();

    // /health/live — solo liveness (nessun tag)
    app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("live") || check.Tags.Count == 0
    });

    // /health/ready — solo readiness (tag "ready")
    app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("ready")
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
