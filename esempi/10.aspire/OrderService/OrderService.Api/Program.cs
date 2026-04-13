using FluentValidation;
using Microsoft.Extensions.Hosting;
using OrderService.Api.Filters;
using OrderService.Api.Middleware;
using OrderService.Api.Validators;
using OrderService.Core;
using OrderService.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Service Defaults: OpenTelemetry + health checks + service discovery + resilienza HTTP
builder.AddServiceDefaults();

builder.Services.AddControllers(options =>
    options.Filters.Add<ValidationFilter>());
builder.Services.AddOpenApi();

builder.Services.AddCore();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.MapControllers();

// Endpoint /health/live e /health/ready da ServiceDefaults
app.MapDefaultEndpoints();

app.Run();
