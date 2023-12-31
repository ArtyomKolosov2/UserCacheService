﻿using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using UserCacheService.Infrastructure.Authentication;

namespace UserCacheService.Handlers;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string Authorization = nameof(Authorization);
    private const string Basic = nameof(Basic);
    private const string InvalidAuthorizationHeader = "Invalid Authorization header";
    private const string MissingAuthorizationHeader = "Missing Authorization header";
    private const string InvalidLoginOrPassword = "Invalid login or password";
    private const string HeaderName = "WWW-Authenticate";
    private const string HeaderMessage = "Basic realm=User Cache Service";
    private const string ClaimType = "name";
    
    private readonly IBasicAuthenticationCredentialsChecker _credentialsChecker;

    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IBasicAuthenticationCredentialsChecker credentialsChecker) 
        : base(options, logger, encoder, clock)
    {
        _credentialsChecker = credentialsChecker;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Otherwise allow anonymous attribute will not work since by default it's used only for authorization in ASP.NET core
        var endpoint = Context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
            return AuthenticateResult.NoResult();
        
        var authorizationHeader = Request.Headers[Authorization].ToString();
        if (authorizationHeader == null || !authorizationHeader.StartsWith(Basic, StringComparison.OrdinalIgnoreCase)) 
            return await Failure(MissingAuthorizationHeader);

        try
        {
            var credentials = GetBasicAuthCredentials(authorizationHeader);
            if (!await _credentialsChecker.CheckCredentials(credentials))
                return await Failure(InvalidLoginOrPassword);
            
            var claims = new[] { new Claim(ClaimType, credentials.Username) };
            var identity = new ClaimsIdentity(claims, Basic);
            var claimsPrincipal = new ClaimsPrincipal(identity);
        
            return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
        }
        catch
        {
            return await Failure(InvalidAuthorizationHeader);
        }
    }

    private static BasicAuthenticationCredentials GetBasicAuthCredentials(string authorizationHeader)
    {
        var token = authorizationHeader.Substring($"{Basic} ".Length).Trim();
        var credentialsFromDecodedString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
        var credentials = credentialsFromDecodedString.Split(':');
        return new BasicAuthenticationCredentials(credentials[0], credentials[1]);
    }

    private Task<AuthenticateResult> Failure(string message)
    {
        Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        Response.Headers.Add(HeaderName, HeaderMessage);

        return Task.FromResult(AuthenticateResult.Fail(message));
    }
}