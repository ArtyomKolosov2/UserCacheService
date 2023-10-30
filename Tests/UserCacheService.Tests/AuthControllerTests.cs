using System.Net;
using System.Xml.Serialization;
using FluentAssertions;
using Flurl.Http;
using Flurl.Http.Xml;
using Tests.EnvironmentBuilder.Integration;
using UserCacheService.Domain.Error;
using UserCacheService.Domain.UserInfo;
using UserCacheService.Dtos;
using Xunit;

namespace UserCacheService.Tests;

[Collection(nameof(IntegrationTestsCollection))]
public class AuthControllerTests : IntegrationTestsBase, IAsyncLifetime
{
    private const string CreateUserUrl = "Auth/CreateUser";
    private const string RemoveUserUrl = "Auth/RemoveUser";
    private const string SetStatusUrl = "Auth/SetStatus";
    
    public AuthControllerTests(IntegrationTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task CreateUser_UserDoesntExistsAndRequestCorrect_UserCreatedAndResponseReturned()
    {
        // Arrange
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

        // Act
        var response = await CreateUserUrl.WithBasicAuth(testCredentials.username, testCredentials.password)
            .PostXmlAsync(createUserRequestDto);
        
        // Assert
        var responseStream = await response.GetStreamAsync();
        var responseDto = new XmlSerializerFactory()
            .CreateSerializer(typeof(CreateUserResponseDto))
            .Deserialize(responseStream) as CreateUserResponseDto;
        
        ValidateResponseContentType(response, XmlContent);
        response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        
        responseDto.Should().NotBeNull();
        responseDto!.User.Should().NotBeNull();
        responseDto.User.Id.Should().Be(createUserRequestDto.User.Id);
        responseDto.User.Status.Should().Be(createUserRequestDto.User.Status);
        responseDto.User.Name.Should().Be(createUserRequestDto.User.Name);
        
        Fixture.DatabaseContext.UserInfos.Should().ContainSingle(x => x.Id == createUserRequestDto.User.Id);
    }
    
    [Fact]
    public async Task CreateUser_UserAlreadyCreatedAndRequestCorrect_UserCreatedAndResponseReturnedWithError()
    {
        // Arrange
        var testCredentials = GetTestCredentials();
        await InsertUserInfoToDb(1, "Test", UserInfoStatus.New);
        var createUserRequestDto = new CreateUserRequestDto
        {
            User = new UserInfoDto
            {
                Id = 1,
                Status = "New",
                Name = "Test"
            }
        };

        // Act
        var response = await CreateUserUrl.WithBasicAuth(testCredentials.username, testCredentials.password)
            .AllowHttpStatus(HttpStatusCode.Conflict)
            .PostXmlAsync(createUserRequestDto);
        
        // Assert
        ValidateResponseContentType(response, XmlContent);
        response.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
        
        var responseStream = await response.GetStreamAsync();
        var responseDto = new XmlSerializerFactory()
            .CreateSerializer(typeof(ErrorResponseDto))
            .Deserialize(responseStream) as ErrorResponseDto;
        
        responseDto.Should().NotBeNull();
        responseDto!.ErrorId.Should().Be((int)ErrorCode.UserAlreadyExists);
        responseDto.Success.Should().BeFalse();
        responseDto.ErrorMessage.Should().Be($"User with id {createUserRequestDto.User.Id} already exist");
    }

    [Fact]
    public async Task RemoveUser_UserExistsAndRequestCorrect_UserDeletedAndResponseReturned()
    {
        // Arrange
        var userInfo = await InsertUserInfoToDb(1, "Test", UserInfoStatus.New);
        
        var testCredentials = GetTestCredentials();
        var createUserRequestDto = new RemoveUserRequestDto
        {
            RemoveUser = new RemoveUserDto { Id = userInfo.Id }
        };

        // Act
        var response = await RemoveUserUrl.WithBasicAuth(testCredentials.username, testCredentials.password)
            .PostJsonAsync(createUserRequestDto);

        // Assert
        ValidateResponseContentType(response, JsonContent);
        var responseDto = await response.GetJsonAsync<RemoveUserResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto.User.Id.Should().Be(createUserRequestDto.RemoveUser.Id);
        responseDto.Message.Should().Be("User was removed");

        Fixture.DatabaseContext.UserInfos.Should().NotContain(x => x.Id == userInfo.Id);
    }
    
    [Fact]
    public async Task RemoveUser_UserDoesntExistsAndRequestCorrect_ErrorResponseReturned()
    {
        // Arrange
        var testCredentials = GetTestCredentials();
        var createUserRequestDto = new RemoveUserRequestDto
        {
            RemoveUser = new RemoveUserDto { Id = 1 }
        };

        // Act
        var response = await RemoveUserUrl.WithBasicAuth(testCredentials.username, testCredentials.password)
            .AllowHttpStatus(HttpStatusCode.NotFound)
            .PostJsonAsync(createUserRequestDto);

        // Assert
        ValidateResponseContentType(response, JsonContent);
        response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        var responseDto = await response.GetJsonAsync<ErrorResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto.Success.Should().BeFalse();
        responseDto.ErrorId.Should().Be((int)ErrorCode.UserNotFound);
        responseDto.ErrorMessage.Should().Be($"Couldn't find user info with id {createUserRequestDto.RemoveUser.Id}");
    }

    [Fact]
    public async Task SetStatus_UserCreatedWithNewStatus_ResponseReturned()
    {
        // Arrange
        var testCredentials = GetTestCredentials();
        var userInfo = await InsertUserInfoToDb(1, "Test", UserInfoStatus.New);
        const UserInfoStatus newStatus = UserInfoStatus.Active;
        
        // Act
        var response = await SetStatusUrl.WithBasicAuth(testCredentials.username, testCredentials.password)
            .PostUrlEncodedAsync(new Dictionary<string, string>
            {
                { "id", userInfo.Id.ToString() },
                { "newStatus", newStatus.ToString() }
            });
        
        // Assert
        ValidateResponseContentType(response, JsonContent);
        response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var responseDto = await response.GetJsonAsync<UserInfoDto>();

        responseDto.Should().NotBeNull();
        responseDto.Id.Should().Be(userInfo.Id);
        responseDto.Status.Should().Be(newStatus.ToString());
        responseDto.Name.Should().Be(userInfo.Name);

        var databaseContext = Fixture.DatabaseContext;
        
        var userInfoFromDatabase = await databaseContext.FindAsync<UserInfo>(userInfo.Id);
        await Fixture.DatabaseContext.Entry(userInfoFromDatabase).ReloadAsync();
        
        userInfoFromDatabase.Should().NotBeNull();
        userInfoFromDatabase!.Status.Should().Be(newStatus);
    }
    
    [Fact]
    public async Task SetStatus_UserDoesntExists_ResponseReturned()
    {
        // Arrange
        var testCredentials = GetTestCredentials();
        const UserInfoStatus newStatus = UserInfoStatus.Active;
        const string id = "1";
        
        // Act
        var response = await SetStatusUrl.WithBasicAuth(testCredentials.username, testCredentials.password)
            .AllowHttpStatus(HttpStatusCode.NotFound)
            .PostUrlEncodedAsync(new Dictionary<string, string>
            {
                { "id", id },
                { "newStatus", newStatus.ToString() }
            });
        
        // Assert
        ValidateResponseContentType(response, JsonContent);
        response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        var responseDto = await response.GetJsonAsync<ErrorResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto.Success.Should().BeFalse();
        responseDto.ErrorId.Should().Be((int)ErrorCode.UserNotFound);
        responseDto.ErrorMessage.Should().Be($"Couldn't find user info with id {id}");
    }
    
    [Fact]
    public async Task SetStatus_UserExistsAndNewStatusHasWrongValue_ResponseReturned()
    {
        // Arrange
        var testCredentials = GetTestCredentials();
        
        await InsertUserInfoToDb(1, "Test", UserInfoStatus.New);
        
        const string invalidStatus = "InvalidStatus";
        const string id = "1";
        
        // Act
        var response = await SetStatusUrl.WithBasicAuth(testCredentials.username, testCredentials.password)
            .AllowHttpStatus(HttpStatusCode.UnprocessableEntity)
            .PostUrlEncodedAsync(new Dictionary<string, string>
            {
                { "id", id },
                { "newStatus", invalidStatus }
            });
        
        // Assert
        ValidateResponseContentType(response, JsonContent);
        response.StatusCode.Should().Be((int)HttpStatusCode.UnprocessableEntity);
        var responseDto = await response.GetJsonAsync<ErrorResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto.Success.Should().BeFalse();
        responseDto.ErrorId.Should().Be((int)ErrorCode.InvalidUserInfoStatus);
        responseDto.ErrorMessage.Should().Be($"User info status value {invalidStatus} can't be used");
    }
    
    public Task InitializeAsync() => Fixture.InitializeAsync();

    public Task DisposeAsync() => ((IAsyncLifetime)Fixture).DisposeAsync();
}