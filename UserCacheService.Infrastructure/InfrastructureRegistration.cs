using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserCacheService.Domain.UserInfo;
using UserCacheService.Domain.UserInfo.Repository;
using UserCacheService.Infrastructure.Authentication;
using UserCacheService.Infrastructure.Database.Context;
using UserCacheService.Infrastructure.Repositories;

namespace UserCacheService.Infrastructure;

public static class InfrastructureRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(nameof(UserCacheServiceDatabaseContext));
        services.AddDbContext<UserCacheServiceDatabaseContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.AddScoped<IUserInfoRepository, UserInfoRepository>();
        services.AddScoped<IBasicAuthenticationCredentialsChecker, BasicAuthenticationCredentialsChecker>();
        
        return services;
    }
}