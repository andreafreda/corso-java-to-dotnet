// InventoryService — microservizio minimale per dimostrare le chiamate HTTP da OrderService
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// GET /inventory/{productId}/available?quantity={qty}
// Risponde sempre true per semplicità — in un sistema reale interrogherebbe un database
app.MapGet("/inventory/{productId:int}/available", (int productId, int quantity) =>
{
    app.Logger.LogInformation(
        "Verifica disponibilità productId={ProductId} quantity={Quantity}", productId, quantity);

    return Results.Ok(true);
});

app.Run();
