#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Deviot.Hermes.Api/Deviot.Hermes.Api.csproj", "Deviot.Hermes.Api/"]
COPY ["Deviot.Hermes.Application/Deviot.Hermes.Application.csproj", "Deviot.Hermes.Application/"]
COPY ["Deviot.Hermes.Infra.Modbus/Deviot.Hermes.Infra.Modbus.csproj", "Deviot.Hermes.Infra.Modbus/"]
COPY ["Deviot.Hermes.Domain/Deviot.Hermes.Domain.csproj", "Deviot.Hermes.Domain/"]
COPY ["Deviot.Hermes.Infra.SQLite/Deviot.Hermes.Infra.SQLite.csproj", "Deviot.Hermes.Infra.SQLite/"]
RUN dotnet restore "Deviot.Hermes.Api/Deviot.Hermes.Api.csproj"
COPY . .
WORKDIR "/src/Deviot.Hermes.Api"
RUN dotnet build "Deviot.Hermes.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Deviot.Hermes.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Deviot.Hermes.Api.dll"]