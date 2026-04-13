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

### `esempi/6.httpclient` — Typed HttpClient + mini InventoryService

**Capitolo:** 6.1–6.2  
**Base:** 5.ef-azuresql → aggiunge chiamata HTTP verso InventoryService  
**Stato:** funzionante con Docker

**Contenuto:**
- `IInventoryClient` nel Core con `IsAvailableAsync(int productId, int quantity)`
- `InventoryClient` typed client in Infrastructure (pattern `AddHttpClient<TInterface, TImpl>`)
- `InventoryService.Api` nella solution: Minimal API con `GET /inventory/{productId}/available?quantity={qty}` → risponde sempre `true`
- `CreateOrderRequest` esteso con `ProductId` e `Quantity`
- `OrderService.CreateAsync` chiama `IInventoryClient` prima di creare l'ordine; lancia `InvalidOperationException` se non disponibile
- `GlobalExceptionHandler` aggiornato con `InvalidOperationException → 422 Unprocessable Entity`
- `Services:InventoryUrl` in `appsettings.Development.json`

**Packages principali:**
- nessuno aggiuntivo (usa `HttpClient` e `Microsoft.Extensions.Http` built-in)

**Come avviare:**
```bash
docker compose -f esempi/6.httpclient/docker/docker-compose.yml up -d
# Terminale 1 — InventoryService su :5001
dotnet run --project esempi/6.httpclient/OrderService/InventoryService.Api
# Terminale 2 — OrderService su :5000
dotnet run --project esempi/6.httpclient/OrderService/OrderService.Api
```

---

### `esempi/6.servicebus` — Azure Service Bus pub/sub

**Capitolo:** 6.3  
**Base:** 5.ef-azuresql → aggiunge messaggistica asincrona con Service Bus  
**Stato:** funzionante con Docker (emulatore)

**Contenuto:**
- `OrderCreatedEvent` record in `Core/Events`
- `IOrderEventPublisher` interfaccia nel Core
- `OrderEventPublisher` in Infrastructure con `ServiceBusSender` (topic `order-created`)
- `OrderCreatedConsumer` come `BackgroundService` (subscription `inventory-sub`): deserializza, logga, completa il messaggio
- `MessageId` idempotente (`order-{id}-{guid}`)
- `IServiceScopeFactory` nel consumer per risolvere servizi Scoped da un Singleton
- `docker/docker-compose.yml` con `servicebus-emulator` (AMQP :5672, management :9090)
- `docker/servicebus-config.json` con topic `order-created` e subscriptions `inventory-sub`, `notification-sub`
- `ConnectionStrings:ServiceBus` con `UseDevelopmentEmulator=true`

**Packages principali:**
- `Azure.Messaging.ServiceBus`

**Come avviare:**
```bash
docker compose -f esempi/6.servicebus/docker/docker-compose.yml up -d
dotnet run --project esempi/6.servicebus/OrderService/OrderService.Api
```

---

### `esempi/6.outbox` — Outbox pattern

**Capitolo:** 6.4  
**Base:** 6.servicebus → aggiunge consistenza distribuita con tabella Outbox  
**Stato:** funzionante con Docker (emulatore)

**Contenuto:**
- `OutboxMessage` entity in `Core/Entities` (Id, Type, Payload, CreatedAt, ProcessedAt)
- `AddWithOutboxAsync` su `IOrderRepository`: salva ordine e messaggio outbox in un'unica transazione EF Core (doppio `SaveChangesAsync` + `BeginTransactionAsync`)
- `OutboxProcessor` come `BackgroundService`: polling ogni 5s, pubblica i messaggi con `ProcessedAt == null` su topic `order-created`, segna come elaborati
- `MessageId = outbox.Id.ToString()` per deduplicazione idempotente su Service Bus
- Migration `AddOutbox` con tabella `Outbox` (`nvarchar(max)` per il payload)
- `InMemoryOrderRepository.AddWithOutboxAsync` delegato ad `AddAsync` (nessuna transazione reale)
- Serializzazione JSON in Infrastructure — il Core passa `OrderCreatedEvent` come oggetto tipizzato

**Packages principali:**
- `Azure.Messaging.ServiceBus` (ereditato da 6.servicebus)

**Come avviare:**
```bash
docker compose -f esempi/6.outbox/docker/docker-compose.yml up -d
dotnet run --project esempi/6.outbox/OrderService/OrderService.Api
```

---

### `esempi/7.jwt` — JWT auth + AuthServer + endpoint protection

**Capitolo:** 7.3–7.6  
**Base:** 5.ef-azuresql → aggiunge autenticazione JWT con refresh token  
**Stato:** funzionante con Docker

**Contenuto:**
- `JwtSettings` record in `Core/Options` (Key, Issuer, Audience, ExpiryMinutes)
- `TokenService` nel Core con `IOptions<JwtSettings>` — NO `IConfiguration` nel Core
- `RefreshToken` entity e `IRefreshTokenStore` nel Core; `InMemoryRefreshTokenStore` con `ConcurrentDictionary` in Infrastructure
- `AuthService` nel Core con login, refresh (con token rotation), logout; utenti hardcoded con hash SHA256
- `AuthController` con `POST /auth/login`, `POST /auth/refresh`, `POST /auth/logout`
- `TokenResponse(AccessToken, RefreshToken, ExpiresIn)` DTO nel Core
- `OrdersController` con `[Authorize]` a livello di classe; policy `WriteOrders` (Admin) su POST; policy `DeleteOrders` (Admin) su DELETE
- `JwtBearerEvents.OnChallenge` (401) e `OnForbidden` (403) con body Problem Details JSON
- `DeleteAsync` aggiunto a `IOrderService`, `IOrderRepository`, `EfCoreOrderRepository`, `InMemoryOrderRepository`

**Packages principali:**
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `System.IdentityModel.Tokens.Jwt`
- `Microsoft.Extensions.Options` (nel Core)

**Come avviare:**
```bash
docker compose -f esempi/7.jwt/docker/docker-compose.yml up -d
dotnet run --project esempi/7.jwt/OrderService/OrderService.Api
# Login: POST /auth/login  { "email": "admin@example.com", "password": "admin123" }
# Usa il token in: Authorization: Bearer <token>
```

---

### `esempi/8.observability` — Health checks + Serilog + OpenTelemetry

**Capitolo:** 8.1–8.3  
**Base:** 5.ef-azuresql → aggiunge health checks, Serilog, OpenTelemetry con Jaeger  
**Stato:** funzionante con Docker

**Contenuto:**
- Health checks: `AddDbContextCheck<AppDbContext>()` (tag `ready`), custom `OrderServiceHealthCheck` (query `COUNT(*)` su Orders, tag `ready`)
- `/health/live` — risponde sempre se il processo è attivo (nessun tag, predicato `tag.Count == 0`)
- `/health/ready` — verifica DB + query orders (tag `ready`)
- Serilog: `UseSerilog` in `Program.cs`, `WriteTo.Console(new CompactJsonFormatter())`, `Enrich.WithMachineName()`, `Enrich.WithEnvironmentName()`; bootstrap logger prima di `CreateBuilder`
- `app.UseSerilogRequestLogging()` per log strutturato delle richieste HTTP
- OpenTelemetry: `ActivitySource("OrderService")` statica in `OrderService` del Core; span custom in `CreateAsync` con tag `order.customer`, `order.total`, `order.id`; `AddAspNetCoreInstrumentation()`; export OTLP verso Jaeger
- `docker/docker-compose.yml` aggiornato con `jaegertracing/all-in-one` (UI :16686, OTLP gRPC :4317)

**Packages principali:**
- `Serilog.AspNetCore`, `Serilog.Enrichers.Environment`, `Serilog.Formatting.Compact`
- `OpenTelemetry.Extensions.Hosting`, `OpenTelemetry.Instrumentation.AspNetCore`, `OpenTelemetry.Exporter.OpenTelemetryProtocol`
- `Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore`

**Come avviare:**
```bash
docker compose -f esempi/8.observability/docker/docker-compose.yml up -d
dotnet run --project esempi/8.observability/OrderService/OrderService.Api
# Health: GET /health/live  GET /health/ready
# Tracing UI: http://localhost:16686
```

---

### `esempi/8.resilienza-caching` — Polly + Redis cache

**Capitolo:** 8.4–8.5  
**Base:** 6.httpclient → aggiunge resilienza Polly su HTTP e caching Redis  
**Stato:** funzionante con Docker

**Contenuto:**
- Polly: `AddResilienceHandler("inventory-pipeline")` con retry (3 tentativi, backoff esponenziale 200ms) + circuit breaker (50% failure rate, min 5 richieste, open 30s)
- `CachedOrderRepository` decorator di `IOrderRepository`: cache Redis per `GetByIdAsync` (chiave `order:{id}`, TTL 5 min)
- `InventoryClient.IsAvailableAsync` con cache Redis prima della chiamata HTTP (chiave `inventory:{productId}:{quantity}`, TTL 30s)
- Registrazione decorator: `AddScoped<EfCoreOrderRepository>()` + `AddScoped<IOrderRepository>(sp => new CachedOrderRepository(...))`
- `AddStackExchangeRedisCache` con `ConnectionStrings:Redis`
- `docker/docker-compose.yml` aggiornato con `redis:7-alpine` (porta 6379)

**Packages principali:**
- `Microsoft.Extensions.Http.Resilience`
- `Microsoft.Extensions.Caching.StackExchangeRedis`

**Come avviare:**
```bash
docker compose -f esempi/8.resilienza-caching/docker/docker-compose.yml up -d
# Terminale 1 — InventoryService su :5001
dotnet run --project esempi/8.resilienza-caching/OrderService/InventoryService.Api
# Terminale 2 — OrderService su :5000
dotnet run --project esempi/8.resilienza-caching/OrderService/OrderService.Api
```

---

### `esempi/9.tests` — Test unitari, integrazione, Testcontainers

**Capitolo:** 9.1–9.7  
**Base:** 5.ef-azuresql → aggiunge due progetti test xUnit  
**Stato:** compila; unit test eseguibili offline; integration tests con Testcontainers richiedono Docker

**Struttura aggiuntiva:**
```
OrderService.UnitTests/     ← xUnit + Moq + EF InMemory
OrderService.IntegrationTests/ ← xUnit + WebApplicationFactory + Testcontainers
```

**Contenuto:**
- `OrderServiceTests`: 4 test su `OrderService.Core.Services.OrderService` con `Mock<IOrderRepository>`
- `EfCoreOrderRepositoryTests`: 4 test con `DbContextOptionsBuilder.UseInMemoryDatabase`, DB isolato per ogni test (Guid come nome DB)
- `OrdersControllerTests`: `WebApplicationFactory<Program>` che sostituisce SqlServer con InMemory — 3 test HTTP (GET, POST, validazione)
- `OrdersControllerDbTests`: `MsSqlContainer` (azure-sql-edge), `IAsyncLifetime` per start/stop, `db.Database.MigrateAsync()` su container — 2 test HTTP
- `public partial class Program {}` aggiunto al fondo di `Program.cs` per esporre la classe ai test
- `appsettings.Testing.json` con log level `Warning`

**Packages principali (test projects):**
- `Moq`, `Microsoft.EntityFrameworkCore.InMemory` (UnitTests)
- `Microsoft.AspNetCore.Mvc.Testing`, `Microsoft.EntityFrameworkCore.InMemory`, `Testcontainers.MsSql` (IntegrationTests)

**Come eseguire:**
```bash
dotnet test esempi/9.tests/OrderService/OrderService.UnitTests
dotnet test esempi/9.tests/OrderService/OrderService.IntegrationTests  # richiede Docker
```

---

### `esempi/9.nunit` — Test con NUnit (alternativa a xUnit)

**Capitolo:** 9.3  
**Base:** 5.ef-azuresql → aggiunge progetto test NUnit  
**Stato:** funzionante, nessun Docker richiesto

**Struttura aggiuntiva:**
```
OrderService.NUnitTests/    ← NUnit + Moq + EF InMemory
```

**Contenuto:**
- `OrderServiceTests` con `[TestFixture]`, `[SetUp]`, `[Test]` — 4 test su `OrderService.Core.Services.OrderService`
- `EfCoreOrderRepositoryTests` con `[SetUp]`/`[TearDown]` e `UseInMemoryDatabase` — 4 test; DB isolato per test con Guid
- Mock con Moq: `It.IsAny<CancellationToken>()` su tutti i setup
- Campi nullable inizializzati con `null!` (`private Mock<IOrderRepository> _repo = null!;`)
- Asserzioni con `Assert.That(..., Is.EqualTo(...))` (stile NUnit fluente, non `Assert.AreEqual`)

**Packages principali:**
- `NUnit`, `NUnit3TestAdapter`, `NUnit.Analyzers`, `Microsoft.NET.Test.Sdk`
- `Moq`, `Microsoft.EntityFrameworkCore.InMemory`

**Come eseguire:**
```bash
dotnet test esempi/9.nunit/OrderService/OrderService.NUnitTests
```

---

### `esempi/10.docker` — Dockerfile multi-stage + docker-compose completo

**Capitolo:** 10.1–10.2  
**Base:** 5.ef-azuresql → aggiunge Dockerfile multi-stage e docker-compose completo  
**Stato:** funzionante con Docker Desktop

**Contenuto:**
- `OrderService/Dockerfile` multi-stage: `build` (sdk:10.0) → `publish` → `runtime` (aspnet:10.0)
  - Layer caching ottimizzato: copia prima i `.csproj` per il restore, poi il codice
  - Utente non-root: `addgroup app && adduser app`, `USER app`
- `docker/docker-compose.yml` completo:
  - `azuresql` con health check (`sqlcmd SELECT 1`, `start_period: 30s`)
  - `migrate` init container: esegue `--migrate-only`, `depends_on: azuresql (healthy)`, `restart: on-failure`
  - `orderservice`: porta 8080, `depends_on: azuresql (healthy) + migrate (completed_successfully)`
- `docker/docker-compose.override.yml` per sviluppo locale (env Development)
- `--migrate-only` flag in `Program.cs`: chiama `db.Database.MigrateAsync()` e termina

**Packages principali:**
- nessuno (usa solo EF Core già presente)

**Come avviare:**
```bash
# Build + avvio completo
docker compose -f esempi/10.docker/docker/docker-compose.yml up --build
# API: http://localhost:8080/orders
```

---

### `esempi/10.aspire` — .NET Aspire AppHost locale

**Capitolo:** 10.3, 11.4  
**Base:** 5.ef-azuresql → aggiunge ServiceDefaults e AppHost Aspire  
**Stato:** Api + ServiceDefaults compilano; AppHost escluso dalla sln (richiede `Aspire.AppHost.Sdk`)

> **Incongruenza 10.1** — Il corso indica `dotnet workload install aspire`, ma dal .NET 10 SDK il workload Aspire è **deprecato**. Il corretto approccio è usare `Aspire.AppHost.Sdk` come NuGet SDK (`Sdk="Aspire.AppHost.Sdk/9.x"`). La struttura del progetto rimane identica, cambia solo come si referenzia l'SDK.

**Struttura aggiuntiva:**
```
OrderService.ServiceDefaults/   ← compila normalmente
OrderService.AppHost/           ← escluso dalla sln; richiede Aspire.AppHost.Sdk come SDK
```

**Contenuto:**
- `ServiceDefaults/Extensions.cs`: `AddServiceDefaults` (OTEL + health checks + service discovery + Polly standard resilience); `MapDefaultEndpoints` per `/health/live` e `/health/ready`
- `AppHost/Program.cs`: `AddSqlServer("sql").AddDatabase("Default")`, `AddProject<Projects.OrderService_Api>("orderservice").WithReference(sql).WaitFor(sql)`
- `OrderService.Api/Program.cs`: aggiunto `builder.AddServiceDefaults()` e `app.MapDefaultEndpoints()`
- `Projects.*` tipi generati da `Aspire.AppHost.Sdk` tramite MSBuild

**Packages principali:**
- `Aspire.Hosting.AppHost`, `Aspire.Hosting.SqlServer` (nell'AppHost)
- `Microsoft.Extensions.ServiceDiscovery`, `OpenTelemetry.*` (nei ServiceDefaults)

**Come avviare:**
```bash
# Richiede Aspire.AppHost.Sdk — usare Aspire SDK-based AppHost
dotnet run --project esempi/10.aspire/OrderService/OrderService.AppHost
# Dashboard: http://localhost:15888
```

---

### `esempi/11.functions` — Azure Functions isolated worker

**Capitolo:** 11.1  
**Base:** progetto standalone (non dipende da OrderService)  
**Stato:** compila; per l'avvio locale richiede `azure-functions-core-tools@4`

**Struttura:**
```
OrderProcessor/
├── Program.cs                     ← FunctionsApplication.CreateBuilder + DI
├── host.json
├── local.settings.json            ← ServiceBusConnection, AzureWebJobsStorage
└── Functions/
    ├── ProcessOrderFunction.cs    ← HttpTrigger POST /orders/{id}/process
    ├── DailyReportFunction.cs     ← TimerTrigger cron "0 0 2 * * *" con IsPastDue
    └── OrderCreatedConsumerFunction.cs ← ServiceBusTrigger topic "order-created" sub "inventory-sub"
```

**Contenuto:**
- Modello isolated worker (unico supportato in .NET 9/10)
- DI completa via `FunctionsApplication.CreateBuilder(args)` (non `HostBuilder`)
- `HttpTrigger` con route parameter `{id}`, `AuthorizationLevel.Anonymous`, risposta JSON
- `TimerTrigger` con NCRONTAB a 6 campi (`secondi minuti ore ...`), controllo `myTimer.IsPastDue`
- `ServiceBusTrigger` con `Connection = "ServiceBusConnection"` dal `local.settings.json`
- Target framework `net9.0` (Functions v4 non supporta ancora net10.0)

**Packages principali:**
- `Microsoft.Azure.Functions.Worker`
- `Microsoft.Azure.Functions.Worker.Extensions.Http.AspNetCore`
- `Microsoft.Azure.Functions.Worker.Extensions.Timer`
- `Microsoft.Azure.Functions.Worker.Extensions.ServiceBus`
- `Microsoft.Azure.Functions.Worker.Sdk`

**Come avviare:**
```bash
npm install -g azure-functions-core-tools@4 --unsafe-perm true
cd esempi/11.functions/OrderProcessor
func start
```

---

### `esempi/11.aot` — Native AOT

**Capitolo:** 11.2  
**Base:** 5.ef-azuresql → convertito per Native AOT  
**Stato:** compila con 0 errori e 0 warning AOT

**Contenuto:**
- `CreateSlimBuilder(args)` al posto di `CreateBuilder` — endpoint routing minimale
- Minimal API (`MapGroup("/orders")`) al posto dei Controller — i controller usano reflection, non compatibili AOT
- `[JsonSerializable]` source generator: `AppJsonSerializerContext` in `Program.cs`, con `OrderDto`, `List<OrderDto>`, `CreateOrderRequest`, `ProblemDetails`
- `ConfigureHttpJsonOptions` con `TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default)`
- `GlobalExceptionHandler` aggiornato: usa `JsonSerializer.Serialize(problem, AppJsonSerializerContext.Default.ProblemDetails)` e `WriteAsync` (no reflection)
- Validazione manuale nel route handler (FluentValidation rimossa — reflection-heavy)
- Rimosso: `FluentValidation`, `Scalar.AspNetCore`, Controller, Filters, Validators
- `PublishAot=true`, `InvariantGlobalization=true` nel `.csproj`
- Per il publish AOT serve: `dotnet publish -r win-x64` (o la runtime target) + Visual C++ Build Tools

**Packages rimossi:**
- `FluentValidation`, `FluentValidation.DependencyInjectionExtensions`, `Scalar.AspNetCore`

**Come avviare:**
```bash
# Build normale (debug, no AOT compile)
dotnet run --project esempi/11.aot/OrderService/OrderService.Api

# Publish AOT nativo (richiede VC++ toolchain)
dotnet publish esempi/11.aot/OrderService/OrderService.Api -r win-x64 -c Release
```

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
