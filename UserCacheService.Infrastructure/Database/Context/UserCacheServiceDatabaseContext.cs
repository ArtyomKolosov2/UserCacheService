using Microsoft.EntityFrameworkCore;
using UserCacheService.Domain.UserInfo;
using UserCacheService.Infrastructure.Authentication;

namespace UserCacheService.Infrastructure.Database.Context;

public class UserCacheServiceDatabaseContext : DbContext
{
    public virtual DbSet<UserInfo> UserInfos { get; set; }
    
    public virtual DbSet<BasicAuthenticationCredentials> BasicAuthenticationCredentials { get; set; }

    public UserCacheServiceDatabaseContext(DbContextOptions<UserCacheServiceDatabaseContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}