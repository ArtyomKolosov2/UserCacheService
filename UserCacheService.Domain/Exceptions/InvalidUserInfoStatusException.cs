using UserCacheService.Domain.Error;

namespace UserCacheService.Domain.Exceptions;

public class InvalidUserInfoStatusException : ServiceBaseException
{
    public InvalidUserInfoStatusException(string userInfoStatusValue) : base($"User info status value {userInfoStatusValue} can't be used", ErrorCode.InvalidUserInfoStatus)
    {
    }
}