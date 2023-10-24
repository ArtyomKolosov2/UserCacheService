using Microsoft.EntityFrameworkCore;
using UserCacheService.Domain.UserInfo;
using UserCacheService.Domain.UserInfo.Repository;
using UserCacheService.Infrastructure.Database.Context;

namespace UserCacheService.Infrastructure.Repositories;

public class UserInfoRepository : IUserInfoRepository
{
    private readonly UserCacheServiceDatabaseContext _context;

    public UserInfoRepository(UserCacheServiceDatabaseContext context)
    {
        _context = context;
    }
    
    public Task<bool> Any(int id, CancellationToken cancellationToken) => _context.UserInfos.AnyAsync(x => x.Id == id, cancellationToken);

    public async Task<UserInfo> Create(UserInfo userInfo, CancellationToken cancellationToken)
    {
        _context.UserInfos.Add(userInfo);
        await _context.SaveChangesAsync(cancellationToken);

        return userInfo;
    }
    
    public async Task<UserInfo?> GetById(int id, CancellationToken cancellationToken)
    {
        return await _context.UserInfos.FindAsync(new object[] { id }, cancellationToken);
    }
    
    public async Task<UserInfo> Update(UserInfo userInfo, CancellationToken cancellationToken)
    {
        _context.Entry(userInfo).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);

        return userInfo;
    }

    public async Task<UserInfo?> DeleteById(int id, CancellationToken cancellationToken)
    {
        var userInfo = await _context.UserInfos.FindAsync(new object[] { id }, cancellationToken);
        _context.UserInfos.Remove(userInfo);
        await _context.SaveChangesAsync(cancellationToken);

        return userInfo;
    }
}