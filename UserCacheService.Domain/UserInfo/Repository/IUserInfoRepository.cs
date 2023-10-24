namespace UserCacheService.Domain.UserInfo.Repository;

public interface IUserInfoRepository
{
    Task<bool> Any(int id, CancellationToken cancellationToken);
    
    Task<UserInfo> Create(UserInfo userInfo, CancellationToken cancellationToken);
    
    Task<UserInfo?> GetById(int id, CancellationToken cancellationToken);
    
    Task<UserInfo> Update(UserInfo userInfo, CancellationToken cancellationToken);
    
    Task<UserInfo?> DeleteById(int id, CancellationToken cancellationToken);
}