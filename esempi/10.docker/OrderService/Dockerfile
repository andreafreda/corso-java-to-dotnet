# ── Stage 1: restore & build ─────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia solo i csproj prima per sfruttare il layer cache di NuGet
COPY OrderService.Core/OrderService.Core.csproj         OrderService.Core/
COPY OrderService.Infrastructure/OrderService.Infrastructure.csproj OrderService.Infrastructure/
COPY OrderService.Api/OrderService.Api.csproj            OrderService.Api/
RUN dotnet restore OrderService.Api/OrderService.Api.csproj

# Copia il resto del codice sorgente
COPY . .

# ── Stage 2: publish ─────────────────────────────────────────────────────────
RUN dotnet publish OrderService.Api/OrderService.Api.csproj \
    -c Release -o /app/publish --no-restore

# ── Stage 3: runtime ─────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Utente non-root per ridurre la superficie d'attacco
RUN addgroup --system app && adduser --system --ingroup app app

COPY --chown=app:app --from=build /app/publish .

USER app
EXPOSE 8080
ENTRYPOINT ["dotnet", "OrderService.Api.dll"]
