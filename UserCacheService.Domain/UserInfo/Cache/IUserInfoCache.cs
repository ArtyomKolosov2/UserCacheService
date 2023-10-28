namespace UserCacheService.Domain.UserInfo.Cache;

public interface IUserInfoCache
{
    Task<UserInfo> GetUserInfoById(int id, CancellationToken cancellationToken);

    Task RefreshCache(CancellationToken cancellationToken);
}