using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using UserCacheService.Domain.Exceptions;
using UserCacheService.Domain.UserInfo.Cache;
using UserCacheService.Domain.UserInfo.Repository;

namespace UserCacheService.Application.UserInfo.Cache;

/// <summary>
/// User info cache with thread safe implementation
/// It uses ConcurrentDictionary to store data in memory
/// It has no functionality to invalidate cache on external entry update since we assume that automatic update should be enough
/// </summary>
public class UserInfoCache : IUserInfoCache
{
    private readonly SemaphoreSlim _semaphoreSlim = new (1);
    
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    private static ConcurrentDictionary<int, Domain.UserInfo.UserInfo> _concurrentDictionary = new();

    public UserInfoCache(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        RefreshCache(CancellationToken.None).GetAwaiter().GetResult();
    }
    
    public async Task<Domain.UserInfo.UserInfo> GetUserInfoById(int id, CancellationToken cancellationToken)
    {
        var userInfo = _concurrentDictionary.GetValueOrDefault(id);
        userInfo ??= await OnCacheMiss(id, cancellationToken);

        return userInfo;
    }

    public async Task RefreshCache(CancellationToken cancellationToken)
    {
        await _semaphoreSlim.WaitAsync(cancellationToken);
        
        using var scope = _serviceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserInfoRepository>();
        var refreshedUserInfos = await repository.GetAll(cancellationToken);
        _concurrentDictionary = new ConcurrentDictionary<int, Domain.UserInfo.UserInfo>(refreshedUserInfos.ToDictionary(x => x.Id));
        
        _semaphoreSlim.Release();
    }

    private async Task<Domain.UserInfo.UserInfo> OnCacheMiss(int id, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserInfoRepository>();
        var userInfoFromDb = await repository.GetById(id, cancellationToken);
        if (userInfoFromDb is null)
            throw new UserInfoNotFoundException(id);

        _concurrentDictionary.AddOrUpdate(userInfoFromDb.Id, userInfoFromDb, (_, _) => userInfoFromDb);

        return userInfoFromDb;
    }
}