#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Microservices/Services/Basket/Basket.API/Basket.API.csproj", "src/Microservices/Services/Basket/Basket.API/"]
COPY ["src/Framework/Touride.Framework.Api/Touride.Framework.Api.csproj", "src/Framework/Touride.Framework.Api/"]
COPY ["src/Framework/Touride.Framework.Abstractions/Touride.Framework.Abstractions.csproj", "src/Framework/Touride.Framework.Abstractions/"]
COPY ["src/Framework/Touride.Framework.Auth/Touride.Framework.Auth.csproj", "src/Framework/Touride.Framework.Auth/"]
COPY ["src/Framework/Touride.Framework.IoC/Touride.Framework.IoC.csproj", "src/Framework/Touride.Framework.IoC/"]
COPY ["src/Framework/Touride.Framework.Client/Touride.Framework.Client.csproj", "src/Framework/Touride.Framework.Client/"]
COPY ["src/Framework/Touride.Framework.Dapr/Touride.Framework.Dapr.csproj", "src/Framework/Touride.Framework.Dapr/"]
COPY ["src/Framework/Touride.Framework.DataAudit.Common/Touride.Framework.DataAudit.Common.csproj", "src/Framework/Touride.Framework.DataAudit.Common/"]
COPY ["src/Framework/Touride.Framework.DataAudit.Elasticsearch/Touride.Framework.DataAudit.Elasticsearch.csproj", "src/Framework/Touride.Framework.DataAudit.Elasticsearch/"]
COPY ["src/Framework/Touride.Framework.DataAudit.RabbitMq/Touride.Framework.DataAudit.RabbitMq.csproj", "src/Framework/Touride.Framework.DataAudit.RabbitMq/"]
COPY ["src/Framework/Touride.Framework.Secret/Touride.Framework.Secret.csproj", "src/Framework/Touride.Framework.Secret/"]
COPY ["src/Framework/Touride.Framework.Data/Touride.Framework.Data.csproj", "src/Framework/Touride.Framework.Data/"]
COPY ["src/Framework/Touride.Framework.Localization/Touride.Framework.Localization.csproj", "src/Framework/Touride.Framework.Localization/"]
COPY ["src/Framework/Touride.Framework.Logging.Serilog/Touride.Framework.Logging.Serilog.csproj", "src/Framework/Touride.Framework.Logging.Serilog/"]
COPY ["src/Framework/Touride.Framework.Logging/Touride.Framework.Logging.csproj", "src/Framework/Touride.Framework.Logging/"]
COPY ["src/Framework/Touride.Framework.Monitoring/Touride.Framework.Monitoring.csproj", "src/Framework/Touride.Framework.Monitoring/"]
COPY ["src/Framework/Touride.Framework.TaskScheduling.Hangfire.Autofac/Touride.Framework.TaskScheduling.Hangfire.Autofac.csproj", "src/Framework/Touride.Framework.TaskScheduling.Hangfire.Autofac/"]
COPY ["src/Framework/Touride.Framework.TaskScheduling.Hangfire/Touride.Framework.TaskScheduling.Hangfire.csproj", "src/Framework/Touride.Framework.TaskScheduling.Hangfire/"]
COPY ["src/Framework/Touride.Framework.Validation/Touride.Framework.Validation.csproj", "src/Framework/Touride.Framework.Validation/"]
COPY ["src/Microservices/Services/Basket/Basket.Application/Basket.Application.csproj", "src/Microservices/Services/Basket/Basket.Application/"]
COPY ["src/Microservices/Services/Basket/Basket.Abstraction/Basket.Abstraction.csproj", "src/Microservices/Services/Basket/Basket.Abstraction/"]
RUN dotnet restore "src/Microservices/Services/Basket/Basket.API/Basket.API.csproj"
COPY . .
WORKDIR "/src/src/Microservices/Services/Basket/Basket.API"
RUN dotnet build "Basket.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Basket.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.API.dll"]