# ─────────────────────────────────────────────────────────
# WEB ŞUBE 2.0 — Multi-stage Dockerfile
# Proje: MyApp.Web (Razor Pages / .NET 8)
# ─────────────────────────────────────────────────────────

# ── STAGE 1: BUILD ────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Node.js kur (Tailwind CSS derleme için)
RUN apt-get update \
    && apt-get install -y --no-install-recommends nodejs npm \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src

# npm bağımlılıklarını kur (package.json değişmediği sürece cache korunur)
# npm install kullanılıyor: lockfile Windows'ta oluşturulduğundan npm ci
# platform-specific binary'leri yanlış çözüyor (@tailwindcss/oxide)
COPY package*.json ./
RUN npm install

# Önce proje dosyalarını kopyala → NuGet cache korunur
COPY *.sln ./
COPY *.csproj ./
RUN dotnet restore

# Kaynak kodu kopyala
COPY . .

# Tailwind CSS + JS kütüphaneleri derle
RUN npm run build

# .NET publish
RUN dotnet publish \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false

# ── STAGE 2: RUNTIME ──────────────────────────────────────
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