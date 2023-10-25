using Microsoft.EntityFrameworkCore;
using UserCacheService.Domain.UserInfo;

namespace UserCacheService.Infrastructure.Database.Context;

public class UserCacheServiceDatabaseContext : DbContext
{
    public virtual DbSet<UserInfo> UserInfos { get; set; }
    
    public UserCacheServiceDatabaseContext(DbContextOptions<UserCacheServiceDatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserInfo>().Property(x => x.Status).HasConversion<string>();
    }
}