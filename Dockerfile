# -----------------------------
# BASE IMAGE (small Mariner OS)
# -----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5077

# Create non-root user safely
RUN adduser --uid 1001 --disabled-password appuser
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WarehouseManagerServer.csproj", "./"]
RUN dotnet restore "WarehouseManagerServer.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "./WarehouseManagerServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WarehouseManagerServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WarehouseManagerServer.dll"]
