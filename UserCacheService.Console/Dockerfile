FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UserCacheService.Console/UserCacheService.Console.csproj", "UserCacheService.Console/"]
RUN dotnet restore "UserCacheService.Console/UserCacheService.Console.csproj"
COPY . .
WORKDIR "/src/UserCacheService.Console"
RUN dotnet build "UserCacheService.Console.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UserCacheService.Console.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserCacheService.Console.dll"]
