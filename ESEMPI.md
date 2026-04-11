# Esempi del corso

Ogni esempio è una solution `OrderService` autonoma con la stessa architettura a tre layer:
`OrderService.Api` · `OrderService.Core` · `OrderService.Infrastructure`

---

## Esempi presenti

### `esempi/4.2` — Primo progetto Web API

**Capitolo:** 4.2 — Creare il primo progetto  
**Base:** template `dotnet new webapi`  
**Stato:** scaffold iniziale, nessun codice applicativo

**Contenuto:**
- `Program.cs` con il WeatherForecast endpoint di default
- Progetti `Core` e `Infrastructure` creati ma vuoti
- Nessun docker-compose

**Come avviare:**
```bash
dotnet run --project esempi/4.2/OrderService/OrderService.Api
```

---

### `esempi/4.10` — OrderService completo in-memory

**Capitolo:** 4.3–4.10 — Architettura e pattern  
**Base:** 4.2 → clean architecture completa con in-memory repo  
**Stato:** funzionante, nessun database

**Contenuto:**
- `OrdersController` con `GET /orders`, `GET /orders/{id}`, `POST /orders`, `PUT /orders/{id}/confirm`, `DELETE /orders/{id}`
- `IOrderService` / `OrderService` nel Core
- `IOrderRepository` / `InMemoryOrderRepository` in Infrastructure
- `CreateOrderRequestValidator` (FluentValidation)
- `ValidationFilter` come action filter
- `GlobalExceptionHandler` (middleware) con Problem Details
- Domain exceptions: `OrderNotFoundException`, `OrderAlreadyConfirmedException`
- Scalar API reference su `/scalar/v1`

**Packages principali:**
- `FluentValidation` + `FluentValidation.DependencyInjectionExtensions`
- `Microsoft.AspNetCore.OpenApi` + `Scalar.AspNetCore`

**Come avviare:**
```bash
dotnet run --project esempi/4.10/OrderService/OrderService.Api
# API: http://localhost:5000
# Scalar: http://localhost:5000/scalar/v1
```

---

### `esempi/5.ef-azuresql` — EF Core + Azure SQL Edge

**Capitolo:** 5.1–5.2 — EF Core con SQL Server  
**Base:** 4.10 → sostituisce InMemoryRepository con EF Core  
**Stato:** funzionante con Docker

**Contenuto:**
- `AppDbContext` + `EfCoreOrderRepository`
- Migration `InitialCreate` già generata
- `InMemoryOrderRepository` mantenuto come fallback di test
- `docker/docker-compose.yml` con `azure-sql-edge:latest`

**Packages principali:**
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Design`

**Come avviare:**
```bash
docker compose -f esempi/5.ef-azuresql/docker/docker-compose.yml up -d
dotnet run --project esempi/5.ef-azuresql/OrderService/OrderService.Api
```

> **Nota:** Questo esempio è la base di partenza per tutti gli esempi successivi dei capitoli 6–8.

---

### `esempi/5.ef-postgresql` — EF Core + PostgreSQL

**Capitolo:** 5.3 — EF Core con PostgreSQL  
**Base:** 5.ef-azuresql → cambia provider da SqlServer a Npgsql  
**Stato:** funzionante con Docker

**Contenuto:**
- Identico a `5.ef-azuresql` nel Core e nell'API
- `AppDbContext` configurato con `UseNpgsql`
- Migration `InitialCreate` con sintassi PostgreSQL
- `docker/docker-compose.yml` con `postgres:16-alpine`

**Packages principali:**
- `Npgsql.EntityFrameworkCore.PostgreSQL`

**Come avviare:**
```bash
docker compose -f esempi/5.ef-postgresql/docker/docker-compose.yml up -d
dotnet run --project esempi/5.ef-postgresql/OrderService/OrderService.Api
```

---

### `esempi/5.dapper` — Dapper + Azure SQL

**Capitolo:** 5.4 — Dapper come alternativa a EF Core  
**Base:** 5.ef-azuresql → rimuove EF Core, aggiunge Dapper  
**Stato:** funzionante con Docker

**Contenuto:**
- `DapperOrderRepository` con query SQL raw (no ORM)
- Nessun `AppDbContext`, nessuna migration (schema gestito manualmente)
- `Microsoft.Data.SqlClient` per la connessione diretta
- `docker/docker-compose.yml` con `azure-sql-edge:latest`

**Packages principali:**
- `Dapper`
- `Microsoft.Data.SqlClient`

**Come avviare:**
```bash
docker compose -f esempi/5.dapper/docker/docker-compose.yml up -d
dotnet run --project esempi/5.dapper/OrderService/OrderService.Api
```

---

### `esempi/5.cosmos` — Cosmos DB emulator

**Capitolo:** 5.5 — Azure Cosmos DB  
**Base:** 4.10 → introduce document model e Cosmos SDK  
**Stato:** funzionante con emulatore Docker

**Contenuto:**
- `CosmosOrderRepository` con `Container.UpsertItemAsync`
- `OrderDocument` come documento Cosmos (mapping da/verso `Order` entity)
- `Newtonsoft.Json` per la serializzazione Cosmos
- `docker/docker-compose.yml` con l'emulatore Cosmos

**Packages principali:**
- `Microsoft.Azure.Cosmos`
- `Newtonsoft.Json`

**Come avviare:**
```bash
docker compose -f esempi/5.cosmos/docker/docker-compose.yml up -d
dotnet run --project esempi/5.cosmos/OrderService/OrderService.Api
```

---

### `esempi/5.mongodb` — MongoDB

**Capitolo:** 5.6 — MongoDB  
**Base:** 4.10 → introduce document model e MongoDB Driver  
**Stato:** funzionante con Docker

**Contenuto:**
- `MongoOrderRepository` con `IMongoCollection<OrderDocument>`
- `OrderDocument` con `BsonId`, `BsonElement` attributes
- `docker/docker-compose.yml` con `mongo:7`

**Packages principali:**
- `MongoDB.Driver`

**Come avviare:**
```bash
docker compose -f esempi/5.mongodb/docker/docker-compose.yml up -d
dotnet run --project esempi/5.mongodb/OrderService/OrderService.Api
```

---

## Develop plan — esempi da costruire

Gli esempi seguenti non sono ancora presenti. Ogni entry specifica: la base di partenza, i package da aggiungere, le modifiche al codice e cosa dimostra.

---

### `esempi/6.httpclient` — Typed HttpClient e service discovery

**Capitolo:** 6.1–6.2  
**Base:** copia di `5.ef-azuresql`  
**Prerequisito docker:** `azure-sql-edge` (già presente)

**Cosa aggiungere:**
- `CreateOrderRequest` aggiornato con `ProductId` e `Quantity`
- `IInventoryClient` nel Core (`Task<bool> IsAvailableAsync(int productId, int quantity)`)
- `InventoryClient` in Infrastructure con `IHttpClientFactory` / typed client
- Mini `InventoryService` nella solution: progetto ASP.NET Core minimale con `GET /inventory/{id}/available?quantity={qty}` che restituisce `true`/`false`
- `IOrderService.CreateAsync` chiama `IInventoryClient` prima di creare l'ordine
- Registrazione: `builder.Services.AddHttpClient<IInventoryClient, InventoryClient>()`

**Packages da aggiungere:**
- nessuno (usa `HttpClient` built-in)

---

### `esempi/6.servicebus` — Azure Service Bus pub/sub

**Capitolo:** 6.3  
**Base:** copia di `6.httpclient`  
**Prerequisito docker:** aggiungi `servicebus-emulator` al docker-compose

**Cosa aggiungere:**
- `IOrderEventPublisher` nel Core con `PublishOrderCreatedAsync(Order order)`
- `OrderCreatedEvent` record nel Core
- `OrderEventPublisher` in Infrastructure con `ServiceBusSender`
- `OrderCreatedConsumer` come `BackgroundService` (sottoscrizione, logga il messaggio e completa)
- `servicebus-config.json` per l'emulatore (definizione queue/topic `order-created`)
- `docker/docker-compose.yml` aggiornato con `mcr.microsoft.com/azure-messaging/servicebus-emulator:latest`

**Packages da aggiungere:**
- `Azure.Messaging.ServiceBus`

---

### `esempi/6.outbox` — Outbox pattern

**Capitolo:** 6.4  
**Base:** copia di `6.servicebus`  
**Prerequisito docker:** stesso di `6.servicebus`

**Cosa aggiungere:**
- `OutboxMessage` entity nel Core
- `AddWithOutboxAsync` su `IOrderRepository` (salva ordine + outbox in una transazione)
- Migration `AddOutbox` con tabella `OutboxMessages`
- `OutboxProcessor` come `BackgroundService` (legge messaggi non inviati, pubblica, segna come elaborati)
- `IOrderEventPublisher` usato dall'`OutboxProcessor`

**Packages da aggiungere:**
- nessuno (usa EF Core già presente)

---

### `esempi/7.jwt` — JWT auth + AuthServer + endpoint protection

**Capitolo:** 7.3–7.6  
**Base:** copia di `5.ef-azuresql`  
**Prerequisito docker:** `azure-sql-edge` (già presente)

**Cosa aggiungere:**
- `JwtSettings` record in `Core/Options` (Issuer, Audience, Key, ExpiryMinutes)
- `TokenService` con `IOptions<JwtSettings>` — NO `IConfiguration` nel Core
- `IRefreshTokenStore` nel Core, `InMemoryRefreshTokenStore` con `ConcurrentDictionary` in Infrastructure
- `AuthService` con login, refresh, logout
- `AuthController` con `/auth/login` (restituisce `AccessToken` + `RefreshToken`), `/auth/refresh`, `/auth/logout`
- Utenti hardcoded in-memory con hash SHA256 (nota BCrypt in produzione)
- `OrdersController` con `[Authorize]` a livello di classe, policy `WriteOrders` e `DeleteOrders`
- `JwtBearerEvents` per 401/403 con body Problem Details JSON

**Packages da aggiungere:**
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `System.IdentityModel.Tokens.Jwt`

---

### `esempi/8.observability` — Health checks + Serilog + OpenTelemetry

**Capitolo:** 8.1–8.3  
**Base:** copia di `5.ef-azuresql`  
**Prerequisito docker:** aggiungi Jaeger al docker-compose

**Cosa aggiungere:**
- Health checks: `AddDbContextCheck<AppDbContext>()`, custom `OrderServiceHealthCheck` (query lightweight), `/health/live` con tag `"live"` e `/health/ready` con tag `"ready"`
- Serilog: `UseSerilog` in `Program.cs`, `WriteTo.Console(new CompactJsonFormatter())`, `Enrich.WithMachineName()`, `Enrich.WithEnvironmentName()`
- OpenTelemetry: `ActivitySource("OrderService")`, span custom in `OrderService.CreateAsync`, esportazione OTLP verso Jaeger
- `docker/docker-compose.yml` aggiornato con Jaeger (`jaegertracing/all-in-one:latest`, porte 16686 + 4317)

**Packages da aggiungere:**
- `Serilog.AspNetCore`
- `Serilog.Enrichers.Environment` ← **indispensabile** per `WithMachineName`/`WithEnvironmentName`
- `Serilog.Formatting.Compact`
- `OpenTelemetry.Extensions.Hosting`
- `OpenTelemetry.Instrumentation.AspNetCore`
- `OpenTelemetry.Exporter.OpenTelemetryProtocol`

---

### `esempi/8.resilienza-caching` — Polly + Redis cache

**Capitolo:** 8.4–8.5  
**Base:** copia di `6.httpclient` (ha già `IInventoryClient`)  
**Prerequisito docker:** aggiungi Redis al docker-compose

**Cosa aggiungere:**
- Polly: `.AddStandardResilienceHandler()` su `IInventoryClient`, `AddResilienceHandler` custom con retry (3 tentativi, backoff esponenziale) + circuit breaker (5 errori → open 30s)
- `CachedOrderRepository` come decorator di `IOrderRepository` con `IDistributedCache` (chiave `order:{id}`, TTL 5 min)
- `IDistributedCache` su Redis per `InventoryClient.IsAvailableAsync` (chiave `inventory:{productId}:{quantity}`, TTL 30s)
- Registrazione: `builder.Services.AddStackExchangeRedisCache(...)` + decorator pattern per il repository
- `docker/docker-compose.yml` aggiornato con `redis:7-alpine`

**Packages da aggiungere:**
- `Microsoft.Extensions.Http.Resilience`
- `Microsoft.Extensions.Caching.StackExchangeRedis`

---

### `esempi/9.tests` — Test unitari, integrazione, Testcontainers

**Capitolo:** 9.1–9.7  
**Base:** copia di `5.ef-azuresql` + aggiunta di progetti test  
**Prerequisito docker:** Testcontainers gestisce i container autonomamente

**Struttura aggiuntiva:**
```
OrderService.UnitTests/     ← xUnit, Moq
OrderService.IntegrationTests/ ← xUnit, WebApplicationFactory, Testcontainers
```

**Cosa aggiungere:**
- Unit test `OrderServiceTests` con Moq per `IOrderRepository`
- Unit test `EfCoreOrderRepositoryTests` con `InMemory` provider
- Integration test con `WebApplicationFactory<Program>` + override servizi
- Integration test con `MsSqlContainer` (Testcontainers) per test su database reale
- `TestTokenHelper` per generare JWT di test
- Profilo `Testing` in `appsettings.Testing.json`

**Packages da aggiungere (test projects):**
- `xunit` + `xunit.runner.visualstudio` + `Microsoft.NET.Test.Sdk`
- `Moq`
- `Microsoft.AspNetCore.Mvc.Testing`
- `Testcontainers.MsSql`

---

### `esempi/10.docker` — Dockerfile multi-stage + docker-compose completo

**Capitolo:** 10.1–10.2  
**Base:** copia di `5.ef-azuresql` (o `8.observability`)  
**Prerequisito:** Docker Desktop

**Cosa aggiungere:**
- `Dockerfile` multi-stage: `build` → `publish` → `runtime` (immagine `aspnet:10.0`)
  - `COPY --chown=app:app`, `USER app`, layer caching ottimizzato
- `docker-compose.yml` completo con `azuresql`, `servicebus`, `redis`, `orderservice`
  - `depends_on` con `condition: service_healthy`
  - health check su SQL Server con `start_period: 30s`
- `docker-compose.override.yml` per sviluppo locale
- Supporto `--migrate-only` in `Program.cs` per init container migrations

**Packages da aggiungere:**
- nessuno

---

### `esempi/10.aspire` — .NET Aspire AppHost locale

**Capitolo:** 10.3, 11.4  
**Base:** copia di `5.ef-azuresql` + nuovi progetti Aspire  
**Prerequisito:** `dotnet workload install aspire`

**Struttura aggiuntiva:**
```
OrderService.AppHost/
OrderService.ServiceDefaults/
```

**Cosa aggiungere:**
- `AppHost/Program.cs` con `AddSqlServer` + `AddDatabase("Default")`, `AddRedis`, `AddProject<Projects.OrderService_Api>` con `.WithReference`
- `ServiceDefaults/Extensions.cs` con `AddServiceDefaults` (OpenTelemetry + health checks + service discovery + Polly)
- `AddServiceDefaults()` in `OrderService.Api/Program.cs`
- `MapDefaultEndpoints()` per `/health/live` e `/health/ready`

**Packages da aggiungere:**
- `Aspire.Hosting` (nel AppHost)
- `Microsoft.Extensions.ServiceDiscovery` (nei ServiceDefaults)

---

### `esempi/11.functions` — Azure Functions isolated worker

**Capitolo:** 11.1  
**Base:** nuovo progetto `func init` (non dipende da OrderService)  
**Prerequisito:** `npm install -g azure-functions-core-tools@4`

**Struttura:**
```
OrderProcessor/
├── Program.cs
├── Functions/
│   ├── ProcessOrderFunction.cs    ← HttpTrigger POST /orders/{id}/process
│   ├── DailyReportFunction.cs     ← TimerTrigger (cron 0 0 2 * * *)
│   └── OrderCreatedConsumerFunction.cs ← ServiceBusTrigger
└── local.settings.json
```

**Cosa implementare:**
- Modello isolated worker (unico supportato in .NET 10)
- DI completa in `Program.cs` con `HostBuilder`
- `HttpTrigger` con route parameter e lettura body JSON
- `TimerTrigger` con controllo `IsPastDue`
- `ServiceBusTrigger` con connessione tramite `local.settings.json`

**Packages da aggiungere:**
- `Microsoft.Azure.Functions.Worker`
- `Microsoft.Azure.Functions.Worker.Extensions.Http`
- `Microsoft.Azure.Functions.Worker.Extensions.Timer`
- `Microsoft.Azure.Functions.Worker.Extensions.ServiceBus`
- `Microsoft.Azure.Functions.Worker.ApplicationInsights`

---

### `esempi/11.aot` — Native AOT

**Capitolo:** 11.2  
**Base:** copia di `5.ef-azuresql` (senza Dapper, AOT non compatibile)  
**Prerequisito:** toolchain nativa (Visual C++ per Windows)

**Cosa modificare:**
- `Program.cs`: `WebApplication.CreateSlimBuilder(args)` invece di `CreateBuilder`
- `[JsonSerializable]` source generator per tutti i tipi serializzati (request/response DTOs)
- `PublishAot=true`, `InvariantGlobalization=true` nel `.csproj`
- `Dockerfile` aggiornato: build con `sdk:10.0` → runtime `runtime-deps:10.0` (non `aspnet`)
- Rimuovere o sostituire ogni libreria non AOT-compatibile (reflection-heavy)

**Packages da aggiungere:**
- nessuno (si rimuove `Scalar.AspNetCore` se non AOT-compatibile)

---

## Dipendenze tra esempi

```
4.2
 └─ 4.10
     └─ 5.ef-azuresql ──────────────────────────┐
     │   ├─ 5.ef-postgresql                      │
     │   ├─ 5.dapper                             │
     │   ├─ 5.cosmos                             │
     │   ├─ 5.mongodb                            │
     │   ├─ 6.httpclient                         │
     │   │   ├─ 6.servicebus                     │
     │   │   │   └─ 6.outbox                     │
     │   │   └─ 8.resilienza-caching             │
     │   ├─ 7.jwt                                │
     │   ├─ 8.observability                      │
     │   ├─ 9.tests                              │
     │   ├─ 10.docker                            │
     │   └─ 10.aspire                            │
     └─ 11.aot ←──────────────────────────────────┘

11.functions    ← progetto standalone (nessuna dipendenza)
```
