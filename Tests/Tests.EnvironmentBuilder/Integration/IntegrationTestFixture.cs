using Flurl.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using UserCacheService.Infrastructure.Database.Context;

namespace Tests.EnvironmentBuilder.Integration;

public class IntegrationTestFixture : IDisposable, IAsyncDisposable
{
    private readonly TestApplicationFactory<Program> _webApplicationFactory;
    private readonly HttpClient _httpClient;
    private readonly TestServer _server;
    private readonly IServiceScope _scope;
    public UserCacheServiceDatabaseContext DatabaseContext { get; init; }

    public IntegrationTestFixture()
    {
        _webApplicationFactory = new TestApplicationFactory<Program>();
        _httpClient = _webApplicationFactory.CreateDefaultClient();
        _server = _webApplicationFactory.Server;

        _scope = _webApplicationFactory.Services.CreateScope();
        DatabaseContext = GetService<UserCacheServiceDatabaseContext>();
        FlurlHttp.Configure(settings => { settings.FlurlClientFactory = new TestClientFactory(_httpClient); });
    }

    public TService GetService<TService>() where TService : notnull =>
        _scope.ServiceProvider.GetRequiredService<TService>();

    public async ValueTask DisposeAsync()
    {
        await DatabaseContext.DisposeAsync();
        await _webApplicationFactory.DisposeAsync();

        _httpClient.Dispose();
        _scope.Dispose();
        _server.Dispose();
    }

    public async void Dispose() => await DisposeAsync();
}