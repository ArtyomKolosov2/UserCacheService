using Microsoft.Extensions.DependencyInjection;
using UserCacheService.Application.UserInfo.Cache;
using UserCacheService.Domain.UserInfo.Cache;

namespace UserCacheService.Application;

public static class ApplicationRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserInfoCache, UserInfoCache>();

        return services;
    }
}