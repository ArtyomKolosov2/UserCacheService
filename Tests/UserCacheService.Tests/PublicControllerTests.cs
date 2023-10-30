using System.Net;
using FluentAssertions;
using Flurl;
using Flurl.Http;
using Tests.EnvironmentBuilder.Integration;
using UserCacheService.Domain.Error;
using UserCacheService.Domain.UserInfo;
using UserCacheService.Dtos;
using Xunit;

namespace UserCacheService.Tests;

[Collection(nameof(IntegrationTestsCollection))]
public class PublicControllerTests : IntegrationTestsBase
{
    private const string UserInfoUrl = "Public/UserInfo";
    
    public PublicControllerTests(IntegrationTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task UserInfo_UserInfoExists_HtmlPageWithUserInfoDataReturned()
    {
        // Arrange
        var userInfo = await InsertUserInfoToDb(1, "Test", UserInfoStatus.New);

        // Act
        var response = await UserInfoUrl
            .SetQueryParam("id", userInfo.Id.ToString())
            .GetStringAsync();

        // Assert
        response.Should().NotBeNull();
        response.Should().Contain(userInfo.Name);
        response.Should().Contain(userInfo.Id.ToString());
        response.Should().Contain(userInfo.Status.ToString());
    }
    
    [Fact]
    public async Task UserInfo_UserInfoDoesntExists_ErrorResponseReturned()
    {
        // Arrange
        const string id = "1";

        // Act
        var response = await UserInfoUrl
            .SetQueryParam("id", id)
            .AllowHttpStatus(HttpStatusCode.NotFound)
            .GetAsync();

        // Assert
        ValidateResponseContentType(response, JsonContent);
        response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        var responseDto = await response.GetJsonAsync<ErrorResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto.Success.Should().BeFalse();
        responseDto.ErrorId.Should().Be((int)ErrorCode.UserNotFound);
        responseDto.ErrorMessage.Should().Be($"Couldn't find user info with id {id}");
    }
}