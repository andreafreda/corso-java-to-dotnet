using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrderService.Api.Filters;
using OrderService.Api.Middleware;
using OrderService.Api.Validators;
using OrderService.Core;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
    options.Filters.Add<ValidationFilter>());
builder.Services.AddOpenApi();

builder.Services.AddCore();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Modalità migrate-only: applica le migrazioni e termina.
// Usato dall'init container in docker-compose.
if (args.Contains("--migrate-only"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    Console.WriteLine("Migrations applied successfully.");
    return;
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.MapControllers();
app.Run();
