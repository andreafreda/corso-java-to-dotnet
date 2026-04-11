using Dapper;
using FluentValidation;
using Microsoft.Data.SqlClient;
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
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Crea il database e la tabella se non esistono — solo in Development
    var connectionString = builder.Configuration.GetConnectionString("Default")!;
    var masterCs = connectionString.Replace("Database=OrderDb", "Database=master");
    await using (var conn = new SqlConnection(masterCs))
    {
        await conn.ExecuteAsync("""
            IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'OrderDb')
                CREATE DATABASE OrderDb;
            """);
    }
    await using (var conn = new SqlConnection(connectionString))
    {
        await conn.ExecuteAsync("""
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Orders]') AND type = 'U')
            CREATE TABLE Orders (
                Id        INT IDENTITY(1,1) PRIMARY KEY,
                Customer  NVARCHAR(100) NOT NULL,
                Total     DECIMAL(18,2) NOT NULL,
                Status    NVARCHAR(50)  NOT NULL DEFAULT 'Pending',
                CreatedAt DATETIME2     NOT NULL DEFAULT GETUTCDATE()
            );
            """);
    }

    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.MapControllers();
app.Run();
