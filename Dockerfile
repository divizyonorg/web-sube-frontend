# ─────────────────────────────────────────────────────────
# WEB ŞUBE 2.0 — Multi-stage Dockerfile
# Proje: MyApp.Web (Razor Pages / .NET 8)
# ─────────────────────────────────────────────────────────

# ── STAGE 1: NODE — CSS & JS derleme ─────────────────────
FROM node:20-slim AS node-build

WORKDIR /src

COPY package.json ./
RUN npm install

COPY . .
RUN npm run build

# ── STAGE 2: .NET BUILD ───────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY *.sln ./
COPY *.csproj ./
RUN dotnet restore

COPY . .

# Node stage'inden derlenmiş CSS ve JS kütüphanelerini al
COPY --from=node-build /src/wwwroot/css/app.css ./wwwroot/css/app.css
COPY --from=node-build /src/wwwroot/lib ./wwwroot/lib

RUN dotnet publish \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false

# ── STAGE 3: RUNTIME ──────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Healthcheck için curl kur
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

# Non-root user oluştur
RUN groupadd -r appgroup && useradd -r -g appgroup -s /sbin/nologin appuser

WORKDIR /app

COPY --from=build --chown=appuser:appgroup /app/publish .

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false

EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

USER appuser

# ⚠️ Proje adı: MyApp.Web
ENTRYPOINT ["dotnet", "MyApp.Web.dll"]
