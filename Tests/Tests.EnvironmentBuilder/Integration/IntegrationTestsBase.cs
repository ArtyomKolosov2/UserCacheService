using FluentAssertions;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserCacheService.Domain.UserInfo;
using UserCacheService.Infrastructure.Authentication;
using Xunit;

namespace Tests.EnvironmentBuilder.Integration;

public class IntegrationTestsBase : IClassFixture<IntegrationTestFixture>
{
    protected const string XmlContent = "application/xml";
    protected const string JsonContent = "application/json";
    
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
    
    protected static void ValidateResponseContentType(IFlurlResponse response, string type)
    {
        response.Headers.Should().ContainSingle(x => x.Name.Contains("content-type", StringComparison.OrdinalIgnoreCase) && x.Value.Contains(type));
    }
}