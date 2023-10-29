using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserCacheService.Domain.UserInfo;
using UserCacheService.Infrastructure.Authentication;
using Xunit;

namespace Tests.EnvironmentBuilder.Integration;

public class IntegrationTestsBase : IClassFixture<IntegrationTestFixture>
{
    protected IntegrationTestFixture Fixture { get; }
    
    protected IntegrationTestsBase(IntegrationTestFixture fixture)
    {
        Fixture = fixture;
    }

    protected (string username, string password) GetTestCredentials()
    {
        var testConfiguration = Fixture.GetService<IConfiguration>();
        var fromConfig = testConfiguration.GetSection(nameof(BasicAuthenticationCredentials));
        var credentialsFromConfig = (fromConfig[nameof(BasicAuthenticationCredentials.Username)],
            fromConfig[nameof(BasicAuthenticationCredentials.Password)]);
        
        return credentialsFromConfig;
    }

    protected async Task<UserInfo> InsertUserInfoToDb(int id, string name, UserInfoStatus status)
    {
        var userInfo = new UserInfo(name)
        {
            Id = id,
            Name = name,
            Status = status
        };
        
        Fixture.DatabaseContext.Add(userInfo);
        await Fixture.DatabaseContext.SaveChangesAsync();

        return userInfo;
    }
}