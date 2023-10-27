using Xunit;

namespace Tests.EnvironmentBuilder.Integration;

public class IntegrationTestsBase : IClassFixture<IntegrationTestFixture>
{
    protected IntegrationTestFixture Fixture { get; }
    
    public IntegrationTestsBase(IntegrationTestFixture fixture)
    {
        Fixture = fixture;
    }
}