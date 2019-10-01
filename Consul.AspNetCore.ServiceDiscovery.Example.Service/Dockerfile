FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Consul.AspNetCore.ServiceDiscovery.Example.Service/Consul.AspNetCore.ServiceDiscovery.Example.Service.csproj", "Consul.AspNetCore.ServiceDiscovery.Example.Service/"]
RUN dotnet restore "Consul.AspNetCore.ServiceDiscovery.Example.Service/Consul.AspNetCore.ServiceDiscovery.Example.Service.csproj"
COPY . .
WORKDIR "/src/Consul.AspNetCore.ServiceDiscovery.Example.Service"
RUN dotnet build "Consul.AspNetCore.ServiceDiscovery.Example.Service.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Consul.AspNetCore.ServiceDiscovery.Example.Service.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Consul.AspNetCore.ServiceDiscovery.Example.Service.dll"]