***

## Indice Completo del Corso — Da Java/Spring a .NET 10 Microservizi


| \# | Macroargomento | Obiettivo | Incremento sul progetto |
| :-- | :-- | :-- | :-- |
| 1 | **Il mondo .NET visto da un developer Java** | Orientamento, differenze ecosistemiche, tooling | — |
| 2 | **C\# per sviluppatori Java** | Transizione linguistica rapida, pattern familiari in chiave C\# | — |
| 3 | **ASP.NET Core: fondamenta del microservizio** | Web API, routing, middleware, DI | — |
| 4 | **Costruiamo il nostro primo microservizio in locale** | Progetto base end-to-end con .NET 10 | 🟢 Creazione del microservizio base |
| 5 | **Persistenza dei dati: EF Core e non solo** | ORM, migrations, transazioni, ottimizzazione query | ➕ Aggiunta database e persistenza |
| 6 | **Comunicazione tra microservizi** | REST, Azure Service Bus, sincrono/asincrono | ➕ Aggiunta chiamate ad altri servizi e messaggistica |
| 7 | **Sicurezza della Web API** | Autenticazione, autorizzazione, JWT | ➕ Aggiunta autenticazione e protezione endpoint |
| 8 | **Resilienza, osservabilità e configurazione** | Health checks, logging, tracing, caching, feature flags | ➕ Aggiunta osservabilità, caching e resilienza |
| 9 | **Testing in .NET** | xUnit, NUnit, integration test, TDD | ➕ Suite di test sull'intero progetto |
| 10 | **Containerizzazione e orchestrazione** | Docker, docker-compose, Kubernetes, CI/CD | ➕ Containerizzazione del microservizio completo |
| 11 | **Oltre i microservizi classici: scenari alternativi** | Azure Functions, Dapr, Native AOT, Aspire | — Scenari autonomi |


***

### 1 — Il mondo .NET visto da un developer Java

| \# | Paragrafo | Analogia Java |
| :-- | :-- | :-- |
| 1.1 | Il runtime: CLR vs JVM | Entrambi compilano in bytecode intermedio, ma il CLR usa IL |
| 1.2 | L'SDK e la CLI (`dotnet` CLI) | Equivalente di `mvn`/`gradle` da terminale |
| 1.3 | Il file `.csproj` e MSBuild | Equivalente del `pom.xml` o `build.gradle` |
| 1.4 | NuGet: il gestore di pacchetti | Equivalente di Maven Central |
| 1.5 | La Solution (`.sln`) e i progetti multipli | Equivalente del multi-module Maven project |
| 1.6 | Tooling: Visual Studio, VS Code, Rider | Equivalente di IntelliJ IDEA / Eclipse |
| 1.7 | Novità .NET 10: file-based programs e `#:package` | Nessun equivalente diretto in Java |
| 1.8 | NuGet e le licenze: open source, freemium e pacchetti commerciali | Equivalente del problema con driver Oracle, Spring Enterprise |


***

### 2 — C\# per sviluppatori Java

| \# | Paragrafo | Analogia Java |
| :-- | :-- | :-- |
| 2.1 | Tipi, variabili e type inference (`var`) | Come `var` in Java 10+ |
| 2.2 | Tipi complessi: date, ora, Guid e altri | Equivalente di `LocalDate`, `UUID`, `Duration` in Java |
| 2.3 | Nullable reference types e `null` safety | Equivalente di `Optional<T>` ma a livello di tipo |
| 2.4 | Properties al posto di getter/setter | Nessun equivalente diretto, in Java serve Lombok |
| 2.5 | Records, struct e classi immutabili | Equivalente di Java Records (Java 16+) |
| 2.6 | Commenti XML e documentazione del codice | Equivalente di Javadoc |
| 2.7 | LINQ: interrogare collezioni in stile funzionale | Equivalente di Stream API |
| 2.8 | Async/await, Task, continuations e CancellationToken | Equivalente di `CompletableFuture`, `thenApply`, task cancellation |
| 2.9 | Pattern matching e switch expressions | Equivalente di switch expressions Java 14+ |
| 2.10 | Extension methods e interfacce | Nessun equivalente diretto in Java |
| 2.11 | Partial class e partial methods | Nessun equivalente diretto in Java |
| 2.12 | Multithreading: Thread, lock, Monitor, Interlocked e SemaphoreSlim | Equivalente di `synchronized`, `ReentrantLock`, `AtomicInteger` |


***

### 3 — ASP.NET Core: fondamenta del microservizio

| \# | Paragrafo | Analogia Java |
| :-- | :-- | :-- |
| 3.1 | Struttura di un progetto ASP.NET Core | Equivalente di un progetto Spring Boot |
| 3.2 | `Program.cs` e il nuovo Minimal API style | Equivalente di `@SpringBootApplication` + main |
| 3.3 | Routing e Controller | Equivalente di `@RestController` e `@RequestMapping` |
| 3.4 | Dependency Injection nativa | Equivalente del container Spring IoC |
| 3.5 | Middleware pipeline | Equivalente dei Filter/Interceptor di Spring |
| 3.6 | Configurazione: `appsettings.json` e Options pattern | Equivalente di `application.yml` + `@ConfigurationProperties` |
| 3.7 | Carter: organizzazione modulare delle Minimal API | Nessun equivalente diretto in Spring |


***

### 4 — Costruiamo il nostro primo microservizio in locale

> 🟢 **Inizio del progetto**: nasce `OrderService`, il microservizio che accompagnerà tutto il corso


| \# | Paragrafo | Note |
| :-- | :-- | :-- |
| 4.1 | Setup ambiente: .NET 10 SDK, VS Code, REST Client | Dalla riga di comando |
| 4.2 | Struttura della Solution: singolo progetto vs multi-progetto (API / Core / Infrastructure) | Scelta architetturale adottata per tutto il corso |
| 4.3 | `dotnet new webapi` e scaffolding della struttura multi-progetto | Creazione della solution con CLI passo passo |
| 4.4 | Endpoint REST: definiamo il contratto dell'API | GET, POST — dati hardcoded, `dotnet run` funziona subito |
| 4.5 | OpenAPI, Scalar e file `.http`: testa subito la tua API | Documentazione interattiva e test manuali |
| 4.6 | DTO e dominio: entità e modelli nel progetto Core | Sposta i modelli al posto giusto |
| 4.7 | Service layer: la logica di business | Equivalente del `@Service` in Spring |
| 4.8 | Wiring: Dependency Injection e `Program.cs` | Tutto registrato, architettura completa |
| 4.9 | Validazione degli input con FluentValidation | Equivalente di Bean Validation / Hibernate Validator |
| 4.10 | Gestione degli errori e Problem Details (RFC 7807) | Equivalente di `@ControllerAdvice` |


***

### 5 — Persistenza dei dati: EF Core e non solo

> ➕ **Incremento**: `OrderService` smette di usare dati in memoria e persiste su database reale

**Parte 1 — dal repository in-memory al database reale**

| \# | Paragrafo | Note |
| :-- | :-- | :-- |
| 5.1 | Azure SQL in locale con Docker | `docker-compose.yml` con Azure SQL Edge — nota per chi preferisce PostgreSQL |
| 5.2 | Entity Framework Core: cos'è e come funziona | Panoramica, tracking, DbContext — equivalente di Hibernate / JPA |
| 5.3 | Code-first migrations | Equivalente di Flyway/Liquibase ma generate dal codice |
| 5.4 | Repository con EF Core: `InMemoryOrderRepository` va in pensione | `dotnet run` funziona con Azure SQL reale |

**Parte 2 — approfondimenti EF Core**

| \# | Paragrafo | Analogia Java |
| :-- | :-- | :-- |
| 5.5 | Gestione delle transazioni: implicite ed esplicite | Equivalente di `@Transactional` in Spring |
| 5.6 | Ottimizzazione delle query: `AsNoTracking`, proiezioni e `AsSplitQuery` | Equivalente di `FetchType`, query hints in JPA |
| 5.7 | Dapper: micro-ORM per query raw | Alternativa leggera, equivalente di MyBatis |

**Parte 3 — NoSQL**

| \# | Paragrafo | Note |
| :-- | :-- | :-- |
| 5.8 | Cosmos DB con .NET: esempio pratico | Azure Cosmos DB SDK, container, operazioni CRUD |
| 5.9 | MongoDB con .NET: esempio pratico | MongoDB.Driver, repository pattern su documento |


***

### 6 — Comunicazione tra microservizi

> ➕ **Incremento**: `OrderService` chiama `InventoryService` in modo sincrono e pubblica eventi su Azure Service Bus


| \# | Paragrafo | Analogia Java |
| :-- | :-- | :-- |
| 6.1 | HttpClient e IHttpClientFactory | Equivalente di `RestTemplate` / `WebClient` |
| 6.2 | Service discovery e pattern client-side | Equivalente di Eureka + Ribbon |
| 6.3 | Messaggistica asincrona con Azure Service Bus | Equivalente di Spring JMS / Spring Cloud Azure |
| 6.4 | Outbox pattern: garantire consistenza distribuita | Pattern architetturale indipendente dal linguaggio |


***

### 7 — Sicurezza della Web API

> ➕ **Incremento**: gli endpoint di `OrderService` vengono protetti con autenticazione JWT


| \# | Paragrafo | Analogia Java |
| :-- | :-- | :-- |
| 7.1 | Il modello di sicurezza in ASP.NET Core: panoramica | Equivalente di Spring Security — architettura a confronto |
| 7.2 | Autenticazione vs Autorizzazione: concetti chiave | `@PreAuthorize`, Roles e Policy in .NET |
| 7.3 | JWT: struttura, generazione e validazione del token | Equivalente di Spring Security + JJWT |
| 7.4 | Implementare un AuthServer minimale in .NET | Equivalente di Spring Authorization Server |
| 7.5 | Proteggere gli endpoint: `[Authorize]` e Policy | Equivalente di `@PreAuthorize` / `SecurityConfig` |
| 7.6 | Refresh token e gestione della sessione | Pattern indipendente dal linguaggio |


***

### 8 — Resilienza, osservabilità e configurazione

> ➕ **Incremento**: `OrderService` diventa production-ready con logging, tracing, caching e circuit breaker


| \# | Paragrafo | Analogia Java |
| :-- | :-- | :-- |
| 8.1 | Health checks (`/health`) | Equivalente di Spring Boot Actuator |
| 8.2 | Logging strutturato con Serilog | Equivalente di Logback + SLF4J |
| 8.3 | Distributed tracing con OpenTelemetry | Equivalente di Micrometer Tracing / Sleuth |
| 8.4 | Resilienza con Polly (retry, circuit breaker) | Equivalente di Resilience4j |
| 8.5 | Caching in .NET: IMemoryCache, IDistributedCache e Redis | Equivalente di Spring Cache + Redis |
| 8.6 | Gestione configurazione centralizzata | Equivalente di Spring Cloud Config |
| 8.7 | Feature Flags con Microsoft.FeatureManagement | Equivalente di Togglz / FF4J in Spring |
| 8.8 | Sicurezza dei dati: redazione log e dati sensibili | Nessun equivalente diretto nativo in Spring |


***

### 9 — Testing in .NET

> ➕ **Incremento**: viene scritta la suite di test completa su tutto ciò che è stato costruito nei capitoli precedenti


| \# | Paragrafo | Analogia Java |
| :-- | :-- | :-- |
| 9.1 | I framework di test in .NET: xUnit, NUnit e MSTest a confronto | Equivalente di JUnit 5 — quale scegliere e perché |
| 9.2 | xUnit in pratica: `[Fact]`, `[Theory]` e struttura dei test | Equivalente di `@Test` e `@ParameterizedTest` |
| 9.3 | NUnit in pratica: `[Test]`, `[TestCase]` e `[SetUp]` | Più simile a JUnit 4 nello stile |
| 9.4 | Mocking con Moq o NSubstitute | Equivalente di Mockito |
| 9.5 | Integration test con `WebApplicationFactory` | Equivalente di `@SpringBootTest` |
| 9.6 | Test del database con Testcontainers | Equivalente di Testcontainers for Java |
| 9.7 | Approccio TDD applicato al microservizio di esempio | Pratica hands-on sull'intero progetto |


***

### 10 — Containerizzazione e orchestrazione

> ➕ **Incremento**: l'intero stack (`OrderService` + `InventoryService` + DB + Service Bus emulato) viene containerizzato


| \# | Paragrafo | Note |
| :-- | :-- | :-- |
| 10.1 | Dockerfile per un'applicazione .NET 10 | Multi-stage build, ottimizzazione immagine |
| 10.2 | docker-compose per l'ambiente locale multi-servizio | Compose con DB, Azure Service Bus emulato e microservizi |
| 10.3 | .NET Aspire: orchestrazione locale moderna | Alternativa a docker-compose, integrata con .NET |
| 10.4 | Cenni su Kubernetes: deploy del microservizio | Deployment, Service, ConfigMap |
| 10.5 | CI/CD con GitHub Actions per .NET | Build, test e push immagine Docker automatizzati |


***

### 11 — Oltre i microservizi classici: scenari alternativi

| \# | Paragrafo | Note |
| :-- | :-- | :-- |
| 11.1 | Azure Functions: serverless con .NET | Trigger HTTP, Queue, Timer |
| 11.2 | Native AOT: compilazione nativa con .NET | Avvio istantaneo, memoria ridotta, limitazioni |
| 11.3 | Dapr: runtime per microservizi portatile | Astrae service discovery, stato, pub/sub |
| 11.4 | .NET Aspire: stack cloud-ready integrato | Osservabilità, service defaults, dashboard locale |
| 11.5 | Quando scegliere cosa: guida decisionale | Matrice microservizi / functions / AOT / Aspire |


***