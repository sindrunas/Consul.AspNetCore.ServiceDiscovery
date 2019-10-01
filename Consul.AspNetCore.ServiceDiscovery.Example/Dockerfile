FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Consul.AspNetCore.ServiceDiscovery.Example/Consul.AspNetCore.ServiceDiscovery.Example.csproj", "Consul.AspNetCore.ServiceDiscovery.Example/"]
RUN dotnet restore "Consul.AspNetCore.ServiceDiscovery.Example/Consul.AspNetCore.ServiceDiscovery.Example.csproj"
COPY . .
WORKDIR "/src/Consul.AspNetCore.ServiceDiscovery.Example"
RUN dotnet build "Consul.AspNetCore.ServiceDiscovery.Example.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Consul.AspNetCore.ServiceDiscovery.Example.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Consul.AspNetCore.ServiceDiscovery.Example.dll"]