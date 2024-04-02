FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BuzzHouse.Processor.Host/BuzzHouse.Processor.Host.csproj", "BuzzHouse.Processor.Host/"]
RUN dotnet restore "BuzzHouse.Processor.Host/BuzzHouse.Processor.Host.csproj"
COPY . .
WORKDIR "/src/BuzzHouse.Processor.Host"
RUN dotnet build "BuzzHouse.Processor.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BuzzHouse.Processor.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BuzzHouse.Processor.Host.dll"]
