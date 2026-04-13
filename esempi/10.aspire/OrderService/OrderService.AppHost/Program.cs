// AppHost/Program.cs
// Richiede Aspire.AppHost.Sdk come SDK del progetto.
// Projects.OrderService_Api è generato da MSBuild tramite Aspire.AppHost.Sdk.

var builder = DistributedApplication.CreateBuilder(args);

// SQL Server — Aspire avvia un container SQL Server automaticamente
var sql = builder.AddSqlServer("sql")
                 .AddDatabase("Default");

// OrderService.Api — riferimento al progetto con dipendenza dal database
// Projects.OrderService_Api viene generato dal package Aspire.AppHost.Sdk
builder.AddProject<Projects.OrderService_Api>("orderservice")
       .WithReference(sql)
       .WaitFor(sql);

builder.Build().Run();
