using Microsoft.Extensions.Configuration;

namespace UserCacheService.Infrastructure.Authentication;

public class BasicAuthenticationCredentialsChecker : IBasicAuthenticationCredentialsChecker
{
    private readonly IConfiguration _configuration;

    public BasicAuthenticationCredentialsChecker(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<bool> CheckCredentials(BasicAuthenticationCredentials credentials)
    {
        var fromConfig = _configuration.GetSection(nameof(BasicAuthenticationCredentials));
        var credentialsFromConfig = new BasicAuthenticationCredentials(fromConfig[nameof(BasicAuthenticationCredentials.Username)],
            fromConfig[nameof(BasicAuthenticationCredentials.Password)]);

        return Task.FromResult(credentials == credentialsFromConfig);
    }
}