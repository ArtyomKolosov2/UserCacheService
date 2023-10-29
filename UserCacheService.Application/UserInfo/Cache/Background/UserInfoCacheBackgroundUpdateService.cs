using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserCacheService.Domain.UserInfo.Cache;

namespace UserCacheService.Application.UserInfo.Cache.Background;

public class UserInfoCacheBackgroundUpdateService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    private readonly TimeSpan _period = TimeSpan.FromMinutes(10);

    public UserInfoCacheBackgroundUpdateService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(_period);
        using var scope = _serviceScopeFactory.CreateScope();
        
        while (!cancellationToken.IsCancellationRequested && await timer.WaitForNextTickAsync(cancellationToken))
        {
            var userInfoCache = scope.ServiceProvider.GetRequiredService<IUserInfoCache>();
            await userInfoCache.RefreshCache(cancellationToken);
        }
    }
}