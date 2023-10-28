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

    protected UserInfo InsertUserInfoToDb(int id, string name, UserInfoStatus status)
    {
        var userInfo = new UserInfo(name)
        {
            Id = id,
            Status = status
        };

        Fixture.DatabaseContext.UserInfos.Add(userInfo);
        Fixture.DatabaseContext.SaveChanges();

        return userInfo;
    }
}