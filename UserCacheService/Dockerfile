FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UserCacheService/UserCacheService.csproj", "UserCacheService/"]
COPY ["UserCacheService.Application/UserCacheService.Application.csproj", "UserCacheService.Application/"]
COPY ["UserCacheService.Domain/UserCacheService.Domain.csproj", "UserCacheService.Domain/"]
COPY ["UserCacheService.Infrastructure/UserCacheService.Infrastructure.csproj", "UserCacheService.Infrastructure/"]
RUN dotnet restore "UserCacheService/UserCacheService.csproj"
COPY . .
WORKDIR "/src/UserCacheService"
RUN dotnet build "UserCacheService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserCacheService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserCacheService.dll"]
