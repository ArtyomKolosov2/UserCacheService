using Xunit;

namespace Tests.EnvironmentBuilder.Integration;

[CollectionDefinition(nameof(IntegrationTestsCollection))]
public class IntegrationTestsCollection : ICollectionFixture<IntegrationTestFixture>
{
}