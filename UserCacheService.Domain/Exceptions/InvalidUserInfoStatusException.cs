using UserCacheService.Domain.Error;

namespace UserCacheService.Domain.Exceptions;

public class InvalidUserInfoStatusException : ServiceBaseException
{
    public InvalidUserInfoStatusException(string userInfoStatusValue) : base($"User info status value \"{(string.IsNullOrEmpty(userInfoStatusValue) ? "null or empty" : userInfoStatusValue)}\" can't be used", ErrorCode.InvalidUserInfoStatus)
    {
    }
}