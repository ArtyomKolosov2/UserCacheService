using FluentAssertions;
using Tests.EnvironmentBuilder.Integration;
using Xunit;

namespace UserCacheService.Tests;

[Collection(nameof(IntegrationTestsCollection))]
public class PublicControllerTests : IntegrationTestsBase
{
    public PublicControllerTests(IntegrationTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public void Test()
    {
        
        true.Should().BeTrue();
    }
}