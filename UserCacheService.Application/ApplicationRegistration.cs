using Microsoft.Extensions.DependencyInjection;
using UserCacheService.Application.UserInfo.Cache;
using UserCacheService.Application.UserInfo.Cache.Update;
using UserCacheService.Application.UserInfo.Create;
using UserCacheService.Application.UserInfo.Remove;
using UserCacheService.Application.UserInfo.Set;
using UserCacheService.Domain.UserInfo.Cache;

namespace UserCacheService.Application;

public static class ApplicationRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserInfoCache, UserInfoCache>();
        services.AddMediatR(x =>
        {
            x.Lifetime = ServiceLifetime.Scoped;
            x.AddBehavior<SetUserStatusCommand>();
            x.AddBehavior<RemoveUserCommand>();
            x.AddBehavior<CreateUserCommand>();
        });

        return services;
    }
}