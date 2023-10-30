using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.Extensions;
using UserCacheService.Application.UserInfo.Cache;
using UserCacheService.Domain.Exceptions;
using UserCacheService.Domain.UserInfo;
using UserCacheService.Domain.UserInfo.Repository;
using Xunit;

namespace UserCacheService.Tests;

public class UserInfoCacheTests
{
    private static UserInfoCache GetSubstitutedUserInfoCache(IUserInfoRepository userInfoRepository)
    {
        var serviceScope = Substitute.For<IServiceScope>();
            
        serviceScope.Configure().ServiceProvider
            .GetService(Arg.Is(typeof(IUserInfoRepository)))
            .Returns(userInfoRepository);
            
        var serviceScopeFactory = Substitute.For<IServiceScopeFactory>();

        serviceScopeFactory.CreateScope()
            .Returns(serviceScope);

        var userInfoCache = new UserInfoCache(serviceScopeFactory);

        return userInfoCache;
    }

    [Fact]
    public async Task GetUserInfoById_CacheInitialized_UserInfoReturned()
    {
        // Arrange
        var userInfoRepository = Substitute.For<IUserInfoRepository>();
        var initialUserInfo = new UserInfo
        {
            Id = 1,
            Name = "Test",
            Status = UserInfoStatus.Active,
        };
        
        userInfoRepository.GetAll(Arg.Any<CancellationToken>())
            .Returns(new [] { initialUserInfo });

        var userInfoCache = GetSubstitutedUserInfoCache(userInfoRepository);
        
        // Act
        var userInfoFromCache = await userInfoCache.GetUserInfoById(initialUserInfo.Id, CancellationToken.None);
        
        // Assert
        userInfoFromCache.Should().Be(initialUserInfo);
    }
    
    [Fact]
    public async Task GetUserInfoById_CacheInitializedAndCacheMissSimulated_UserInfoReturnedAndCallToRepositoryRecorded()
    {
        // Arrange
        var userInfoRepository = Substitute.For<IUserInfoRepository>();
        var initialUserInfo = new UserInfo
        {
            Id = 1,
            Name = "Test",
            Status = UserInfoStatus.Active,
        };

        var cacheMissUserInfo = new UserInfo
        {
            Id = 2,
            Name = "Test",
            Status = UserInfoStatus.Blocked
        };
        
        userInfoRepository.GetAll(Arg.Any<CancellationToken>())
            .Returns(new [] { initialUserInfo });

        userInfoRepository.GetById(Arg.Is(cacheMissUserInfo.Id), Arg.Any<CancellationToken>())
            .Returns(cacheMissUserInfo);

        var userInfoCache = GetSubstitutedUserInfoCache(userInfoRepository);
        
        // Act
        var userInfoFromCache = await userInfoCache.GetUserInfoById(cacheMissUserInfo.Id, CancellationToken.None);
        
        // Assert
        userInfoFromCache.Should().Be(cacheMissUserInfo);
        await userInfoRepository.Received().GetById(Arg.Is(cacheMissUserInfo.Id), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task GetUserInfoById_CacheInitializedAndCacheMissSimulatedAndUserDoesntExist_ExceptionThrown()
    {
        // Arrange
        var userInfoRepository = Substitute.For<IUserInfoRepository>();
        var initialUserInfo = new UserInfo
        {
            Id = 1,
            Name = "Test",
            Status = UserInfoStatus.Active,
        };

        const int id = 100;
        
        userInfoRepository.GetAll(Arg.Any<CancellationToken>())
            .Returns(new [] { initialUserInfo });

        userInfoRepository.GetById(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(_ => Task.FromResult<UserInfo?>(null));

        var userInfoCache = GetSubstitutedUserInfoCache(userInfoRepository);
        
        // Act
        var getUserInfoFromCacheTask = () => userInfoCache.GetUserInfoById(id, CancellationToken.None);
        
        // Assert
        await getUserInfoFromCacheTask.Should().ThrowAsync<UserInfoNotFoundException>().WithMessage($"Couldn't find user info with id {id}");
        await userInfoRepository.Received().GetById(Arg.Any<int>(), Arg.Any<CancellationToken>());
    }
}