using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using UserCacheService.Domain.Exceptions;
using UserCacheService.Domain.UserInfo.Cache;
using UserCacheService.Domain.UserInfo.Repository;

namespace UserCacheService.Application.UserInfo.Cache;

public class UserInfoCache : IUserInfoCache
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    private static ConcurrentDictionary<int, Domain.UserInfo.UserInfo> _hashTable = new();

    public UserInfoCache(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        RefreshCache(CancellationToken.None).GetAwaiter().GetResult();
    }
    
    public async Task<Domain.UserInfo.UserInfo> GetUserInfoById(int id, CancellationToken cancellationToken)
    {
        var userInfo = _hashTable.GetValueOrDefault(id);
        userInfo ??= await OnCacheMiss(id, cancellationToken);

        return userInfo;
    }

    public async Task RefreshCache(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserInfoRepository>();
        var refreshedUserInfos = await repository.GetAll(cancellationToken);
        _hashTable = new ConcurrentDictionary<int, Domain.UserInfo.UserInfo>(refreshedUserInfos.ToDictionary(x => x.Id));
    }

    private async Task<Domain.UserInfo.UserInfo> OnCacheMiss(int id, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserInfoRepository>();
        var userInfo = await repository.GetById(id, cancellationToken);
        if (userInfo is null)
            throw new UserInfoNotFoundException(id);

        return userInfo;
    }
}