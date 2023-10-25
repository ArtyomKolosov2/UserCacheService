using Microsoft.EntityFrameworkCore;
using UserCacheService.Domain.UserInfo;
using UserCacheService.Infrastructure.Database.Context;

namespace UserCacheService.Infrastructure.Database;

public static class DatabaseDataSeeder
{
    public static async Task SeedDatabaseWithDataIfEmpty(UserCacheServiceDatabaseContext databaseContext)
    {
        if (!await databaseContext.UserInfos.AnyAsync())
            databaseContext.UserInfos.AddRange(Enumerable.Range(0, 5).Select(number => new UserInfo($"test {number}")
            {
                Status = UserInfoStatus.New
            }));

        await databaseContext.SaveChangesAsync();
    }
}