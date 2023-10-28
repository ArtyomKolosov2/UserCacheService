using UserCacheService.Domain.Error;

namespace UserCacheService.Domain.Exceptions;

public class UserInfoNotFoundException : ServiceBaseException
{
    public UserInfoNotFoundException(int id) : base($"Couldn't find user info with id {id}", ErrorCode.UserNotFound)
    {
    }
}