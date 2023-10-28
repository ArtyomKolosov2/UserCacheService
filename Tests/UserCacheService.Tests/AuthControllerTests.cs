using FluentAssertions;
using Flurl.Http;
using Flurl.Http.Xml;
using Tests.EnvironmentBuilder.Integration;
using UserCacheService.Domain.UserInfo;
using UserCacheService.Dtos;
using Xunit;

namespace UserCacheService.Tests;

[Collection(nameof(IntegrationTestsCollection))]
public class AuthControllerTests : IntegrationTestsBase
{
    private const string CreateUserUrl = "Auth/CreateUser";
    private const string RemoveUserUrl = "Auth/RemoveUser";
    
    public AuthControllerTests(IntegrationTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Test()
    {
        var testCredentials = GetTestCredentials();
        var createUserRequestDto = new CreateUserRequestDto
        {
            User = new UserInfoDto
            {
                Id = 1,
                Status = "New",
                Name = "Test"
            }
        };

        var response = await CreateUserUrl.WithBasicAuth(testCredentials.username, testCredentials.password)
            .PostXmlAsync(createUserRequestDto);
        
        var responseDto = await response.GetStringAsync();
        responseDto.Should().NotBeNull();
    }

    [Fact] public async Task RemoveUser_UserExistsAndRequestCorrect_UserDeletedAndResponseReturned()
    {
        // Arrange
        var userInfo = InsertUserInfoToDb(1, "Test", UserInfoStatus.New);
        
        var testCredentials = GetTestCredentials();
        var createUserRequestDto = new RemoveUserRequestDto
        {
            RemoveUser = new RemoveUserDto { Id = userInfo.Id }
        };

        // Act
        var response = await RemoveUserUrl.WithBasicAuth(testCredentials.username, testCredentials.password)
            .PostJsonAsync(createUserRequestDto);

        // Assert
        response.Headers.Should().ContainSingle(x => x.Name.Contains("content-type", StringComparison.OrdinalIgnoreCase) && x.Value.Contains("application/json"));
        var responseDto = await response.GetJsonAsync<RemoveUserResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto.User.Id.Should().Be(createUserRequestDto.RemoveUser.Id);
        responseDto.Message.Should().Be("User was removed");

        Fixture.DatabaseContext.UserInfos.Should().NotContain(x => x.Id == userInfo.Id);
    } 
}