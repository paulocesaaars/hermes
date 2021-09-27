#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Deviot.Hermes.Api/Deviot.Hermes.Api.csproj", "Deviot.Hermes.Api/"]
COPY ["Deviot.Hermes.Data/Deviot.Hermes.Data.csproj", "Deviot.Hermes.Data/"]
COPY ["Deviot.Hermes.Business/Deviot.Hermes.Business.csproj", "Deviot.Hermes.Business/"]
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