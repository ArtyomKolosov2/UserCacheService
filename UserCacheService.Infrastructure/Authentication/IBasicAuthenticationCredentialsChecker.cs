namespace UserCacheService.Infrastructure.Authentication;

public interface IBasicAuthenticationCredentialsChecker
{
    Task<bool> CheckCredentials(BasicAuthenticationCredentials credentials);
}