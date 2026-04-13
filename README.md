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
| 1.1 | [Il runtime: CLR vs JVM](capitolo-01/1.1-clr-vs-jvm.md) | Entrambi compilano in bytecode intermedio, ma il CLR usa IL |
| 1.2 | [L'SDK e la CLI (`dotnet` CLI)](capitolo-01/1.2-dotnet-cli.md) | Equivalente di `mvn` da terminale |
| 1.3 | [Il file `.csproj` e MSBuild](capitolo-01/1.3-csproj-msbuild.md) | Equivalente del `pom.xml` |
| 1.4 | [NuGet: il gestore di pacchetti](capitolo-01/1.4-nuget.md) | Equivalente di Maven Central |
| 1.5 | [La Solution (`.sln`) e i progetti multipli](capitolo-01/1.5-solution.md) | Equivalente del multi-module Maven project |
| 1.6 | [Tooling: Visual Studio, VS Code, Rider](capitolo-01/1.6-tooling.md) | Equivalente di IntelliJ IDEA / Eclipse |
| 1.7 | [Novità .NET 10: file-based programs e `#:package`](capitolo-01/1.7-novita-dotnet10.md) | Nessun equivalente diretto in Java |
| 1.8 | [NuGet e le licenze: open source, freemium e pacchetti commerciali](capitolo-01/1.8-licenze-nuget.md) | Equivalente del problema con driver Oracle, Spring Enterprise |


***

### 2 — C\# per sviluppatori Java

| \# | Paragrafo | Analogia Java |
| :-- | :-- | :-- |
| 2.1 | [Tipi, variabili e type inference (`var`)](capitolo-02/2.1-tipi-variabili.md) | Come `var` in Java 10+ |
| 2.2 | [Tipi complessi: date, ora, Guid e altri](capitolo-02/2.2-tipi-complessi.md) | Equivalente di `LocalDate`, `UUID`, `Duration` in Java |
| 2.3 | [Nullable reference types e `null` safety](capitolo-02/2.3-nullable.md) | Equivalente di `Optional<T>` ma a livello di tipo |
| 2.4 | [Properties al posto di getter/setter](capitolo-02/2.4-properties.md) | Nessun equivalente diretto, in Java serve Lombok |
| 2.5 | [Records, struct e classi immutabili](capitolo-02/2.5-records-struct.md) | Equivalente di Java Records (Java 16+) |
| 2.6 | [Commenti XML e documentazione del codice](capitolo-02/2.6-xml-comments.md) | Equivalente di Javadoc |
| 2.7 | [LINQ: interrogare collezioni in stile funzionale](capitolo-02/2.7-linq.md) | Equivalente di Stream API |
| 2.8 | [Async/await, Task, continuations e CancellationToken](capitolo-02/2.8-async-await.md) | Equivalente di `CompletableFuture`, `thenApply`, task cancellation |
| 2.9 | [Pattern matching e switch expressions](capitolo-02/2.9-pattern-matching.md) | Equivalente di switch expressions Java 14+ |
| 2.10 | [Extension methods e interfacce](capitolo-02/2.10-extension-methods.md) | Nessun equivalente diretto in Java |
| 2.11 | [Partial class e partial methods](capitolo-02/2.11-partial.md) | Nessun equivalente diretto in Java |
| 2.12 | [Multithreading: Thread, lock, Monitor, Interlocked e SemaphoreSlim](capitolo-02/2.12-multithreading.md) | Equivalente di `synchronized`, `ReentrantLock`, `AtomicInteger` |


***

### 3 — ASP.NET Core: fondamenta del microservizio

| \# | Paragrafo | Analogia Java |
| :-- | :-- | :-- |
| 3.1 | [Struttura di un progetto ASP.NET Core](capitolo-03/3.1-struttura-progetto.md) | Equivalente di un progetto Spring Boot |
| 3.2 | [`Program.cs` e il nuovo Minimal API style](capitolo-03/3.2-program-cs.md) | Equivalente di `@SpringBootApplication` + main |
| 3.3 | [Routing e Controller](capitolo-03/3.3-routing-controller.md) | Equivalente di `@RestController` e `@RequestMapping` |
| 3.4 | [Dependency Injection nativa](capitolo-03/3.4-dependency-injection.md) | Equivalente del container Spring IoC |
| 3.5 | [Middleware pipeline](capitolo-03/3.5-middleware.md) | Equivalente dei Filter/Interceptor di Spring |
| 3.6 | [Configurazione: `appsettings.json` e Options pattern](capitolo-03/3.6-configurazione.md) | Equivalente di `application.yml` + `@ConfigurationProperties` |
| 3.7 | [Carter: organizzazione modulare delle Minimal API](capitolo-03/3.7-minimal.md) | Nessun equivalente diretto in Spring |


***

### 4 — Costruiamo il nostro primo microservizio in locale

> 🟢 **Inizio del progetto**: nasce `OrderService`, il microservizio che accompagnerà tutto il corso


| \# | Paragrafo | Note |
| :-- | :-- | :-- |
| 4.1 | [Setup ambiente: .NET 10 SDK, VS Code, REST Client](capitolo-04/4.1-setup-ambiente.md) | Dalla riga di comando |
| 4.2 | [Struttura della Solution: singolo progetto vs multi-progetto (API / Core / Infrastructure)](capitolo-04/4.2-struttura-solution.md) | Scelta architetturale adottata per tutto il corso |
| 4.3 | [`dotnet new webapi` e scaffolding della struttura multi-progetto](capitolo-04/4.3-scaffolding.md) | Creazione della solution con CLI passo passo |
| 4.4 | [Endpoint REST: definiamo il contratto dell'API](capitolo-04/4.4-endpoint.md) | GET, POST — dati hardcoded, `dotnet run` funziona subito |
| 4.5 | [OpenAPI, Scalar e file `.http`: testa subito la tua API](capitolo-04/4.5-openapi-http.md) | Documentazione interattiva e test manuali |
| 4.6 | [DTO e dominio: entità e modelli nel progetto Core](capitolo-04/4.6-dominio.md) | Sposta i modelli al posto giusto |
| 4.7 | [Service layer: la logica di business](capitolo-04/4.7-service-layer.md) | Equivalente del `@Service` in Spring |
| 4.8 | [Wiring: Dependency Injection e `Program.cs`](capitolo-04/4.8-wiring.md) | Tutto registrato, architettura completa |
| 4.9 | [Validazione degli input con FluentValidation](capitolo-04/4.9-validazione.md) | Equivalente di Bean Validation / Hibernate Validator |
| 4.10 | [Gestione degli errori e Problem Details (RFC 7807)](capitolo-04/4.10-error-handling.md) | Equivalente di `@ControllerAdvice` |


***

### 5 — Persistenza dei dati: EF Core e non solo

> ➕ **Incremento**: `OrderService` smette di usare dati in memoria e persiste su database reale

**Parte 1 — dal repository in-memory al database reale**

| \# | Paragrafo | Note |
| :-- | :-- | :-- |
| 5.1 | [Azure SQL in locale con Docker](capitolo-05/5.1-azure-sql-docker.md) | `docker-compose.yml` con Azure SQL Edge — nota per chi preferisce PostgreSQL |
| 5.2 | [Entity Framework Core: cos'è e come funziona](capitolo-05/5.2-efcore.md) | Panoramica, tracking, DbContext — equivalente di Hibernate / JPA |
| 5.3 | [Code-first migrations](capitolo-05/5.3-migrations.md) | Equivalente di Flyway/Liquibase ma generate dal codice |
| 5.4 | [Repository con EF Core: `InMemoryOrderRepository` va in pensione](capitolo-05/5.4-repository-efcore.md) | `dotnet run` funziona con Azure SQL reale |

**Parte 2 — approfondimenti EF Core**

| \# | Paragrafo | Analogia Java |
| :-- | :-- | :-- |
| 5.5 | [Gestione delle transazioni: implicite ed esplicite](capitolo-05/5.5-transazioni.md) | Equivalente di `@Transactional` in Spring |
| 5.6 | [Ottimizzazione delle query: `AsNoTracking`, proiezioni e `AsSplitQuery`](capitolo-05/5.6-query-optimization.md) | Equivalente di `FetchType`, query hints in JPA |
| 5.7 | [Dapper: micro-ORM per query raw](capitolo-05/5.7-dapper.md) | Alternativa leggera, equivalente di MyBatis |

**Parte 3 — NoSQL**

| \# | Paragrafo | Note |
| :-- | :-- | :-- |
| 5.8 | [Cosmos DB con .NET: esempio pratico](capitolo-05/5.8-cosmosdb.md) | Azure Cosmos DB SDK, container, operazioni CRUD |
| 5.9 | [MongoDB con .NET: esempio pratico](capitolo-05/5.9-mongodb.md) | MongoDB.Driver, repository pattern su documento |


***

### 6 — Comunicazione tra microservizi

> ➕ **Incremento**: `OrderService` chiama `InventoryService` in modo sincrono e pubblica eventi su Azure Service Bus


| \# | Paragrafo | Analogia Java |
| :-- | :-- | :-- |
| 6.1 | [HttpClient e IHttpClientFactory](capitolo-06/6.1-httpclient.md) | Equivalente di `RestTemplate` / `WebClient` |
| 6.2 | [Service discovery e pattern client-side](capitolo-06/6.2-service-discovery.md) | Equivalente di Eureka + Ribbon |
| 6.3 | [Messaggistica asincrona con Azure Service Bus](capitolo-06/6.3-servicebus.md) | Equivalente di Spring JMS / Spring Cloud Azure |
| 6.4 | [Outbox pattern: garantire consistenza distribuita](capitolo-06/6.4-outbox.md) | Pattern architetturale indipendente dal linguaggio |


***

### 7 — Sicurezza della Web API

> ➕ **Incremento**: gli endpoint di `OrderService` vengono protetti con autenticazione JWT


| \# | Paragrafo | Analogia Java |
| :-- | :-- | :-- |
| 7.1 | [Il modello di sicurezza in ASP.NET Core: panoramica](capitolo-07/7.1-security-overview.md) | Equivalente di Spring Security — architettura a confronto |
| 7.2 | [Autenticazione vs Autorizzazione: concetti chiave](capitolo-07/7.2-authn-authz.md) | `@PreAuthorize`, Roles e Policy in .NET |
| 7.3 | [JWT: struttura, generazione e validazione del token](capitolo-07/7.3-jwt.md) | Equivalente di Spring Security + JJWT |
| 7.4 | [Implementare un AuthServer minimale in .NET](capitolo-07/7.4-authserver.md) | Equivalente di Spring Authorization Server |
| 7.5 | [Proteggere gli endpoint: `[Authorize]` e Policy](capitolo-07/7.5-proteggere-endpoint.md) | Equivalente di `@PreAuthorize` / `SecurityConfig` |
| 7.6 | [Refresh token e gestione della sessione](capitolo-07/7.6-refresh-token.md) | Pattern indipendente dal linguaggio |


***

### 8 — Resilienza, osservabilità e configurazione

> ➕ **Incremento**: `OrderService` diventa production-ready con logging, tracing, caching e circuit breaker


| \# | Paragrafo | Analogia Java |
| :-- | :-- | :-- |
| 8.1 | [Health checks (`/health`)](capitolo-08/8.1-health-checks.md) | Equivalente di Spring Boot Actuator |
| 8.2 | [Logging strutturato con Serilog](capitolo-08/8.2-serilog.md) | Equivalente di Logback + SLF4J |
| 8.3 | [Distributed tracing con OpenTelemetry](capitolo-08/8.3-opentelemetry.md) | Equivalente di Micrometer Tracing / Sleuth |
| 8.4 | [Resilienza con Polly (retry, circuit breaker)](capitolo-08/8.4-polly.md) | Equivalente di Resilience4j |
| 8.5 | [Caching in .NET: IMemoryCache, IDistributedCache e Redis](capitolo-08/8.5-caching.md) | Equivalente di Spring Cache + Redis |
| 8.6 | [Gestione configurazione centralizzata](capitolo-08/8.6-configurazione-centralizzata.md) | Equivalente di Spring Cloud Config |
| 8.7 | [Feature Flags con Microsoft.FeatureManagement](capitolo-08/8.7-feature-flags.md) | Equivalente di Togglz / FF4J in Spring |
| 8.8 | [Sicurezza dei dati: redazione log e dati sensibili](capitolo-08/8.8-data-security.md) | Nessun equivalente diretto nativo in Spring |


***

### 9 — Testing in .NET

> ➕ **Incremento**: viene scritta la suite di test completa su tutto ciò che è stato costruito nei capitoli precedenti


| \# | Paragrafo | Analogia Java |
| :-- | :-- | :-- |
| 9.1 | [I framework di test in .NET: xUnit, NUnit e MSTest a confronto](capitolo-09/9.1-framework-test.md) | Equivalente di JUnit 5 — quale scegliere e perché |
| 9.2 | [xUnit in pratica: `[Fact]`, `[Theory]` e struttura dei test](capitolo-09/9.2-xunit.md) | Equivalente di `@Test` e `@ParameterizedTest` |
| 9.3 | [NUnit in pratica: `[Test]`, `[TestCase]` e `[SetUp]`](capitolo-09/9.3-nunit.md) | Più simile a JUnit 4 nello stile |
| 9.4 | [Mocking con Moq o NSubstitute](capitolo-09/9.4-mocking.md) | Equivalente di Mockito |
| 9.5 | [Integration test con `WebApplicationFactory`](capitolo-09/9.5-integration-test.md) | Equivalente di `@SpringBootTest` |
| 9.6 | [Test del database con Testcontainers](capitolo-09/9.6-testcontainers.md) | Equivalente di Testcontainers for Java |
| 9.7 | [Approccio TDD applicato al microservizio di esempio](capitolo-09/9.7-tdd.md) | Pratica hands-on sull'intero progetto |


***

### 10 — Containerizzazione e orchestrazione

> ➕ **Incremento**: l'intero stack (`OrderService` + `InventoryService` + DB + Service Bus emulato) viene containerizzato


| \# | Paragrafo | Note |
| :-- | :-- | :-- |
| 10.1 | [Dockerfile per un'applicazione .NET 10](capitolo-10/10.1-dockerfile.md) | Multi-stage build, ottimizzazione immagine |
| 10.2 | [docker-compose per l'ambiente locale multi-servizio](capitolo-10/10.2-docker-compose.md) | Compose con DB, Azure Service Bus emulato e microservizi |
| 10.3 | [.NET Aspire: orchestrazione locale moderna](capitolo-10/10.3-aspire.md) | Alternativa a docker-compose, integrata con .NET |
| 10.4 | [Cenni su Kubernetes: deploy del microservizio](capitolo-10/10.4-kubernetes.md) | Deployment, Service, ConfigMap |
| 10.5 | [CI/CD con GitHub Actions per .NET](capitolo-10/10.5-cicd.md) | Build, test e push immagine Docker automatizzati |


***

### 11 — Oltre i microservizi classici: scenari alternativi

| \# | Paragrafo | Note |
| :-- | :-- | :-- |
| 11.1 | [Azure Functions: serverless con .NET](capitolo-11/11.1-azure-functions.md) | Trigger HTTP, Queue, Timer |
| 11.2 | [Native AOT: compilazione nativa con .NET](capitolo-11/11.2-native-aot.md) | Avvio istantaneo, memoria ridotta, limitazioni |
| 11.3 | [Dapr: runtime per microservizi portatile](capitolo-11/11.3-dapr.md) | Astrae service discovery, stato, pub/sub |
| 11.4 | [.NET Aspire: stack cloud-ready integrato](capitolo-11/11.4-aspire.md) | Osservabilità, service defaults, dashboard locale |
| 11.5 | [Quando scegliere cosa: guida decisionale](capitolo-11/11.5-guida-decisionale.md) | Matrice microservizi / functions / AOT / Aspire |


***
