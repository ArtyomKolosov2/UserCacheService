using UserCacheService.Domain.Error;

namespace UserCacheService.Domain.Exceptions;

public class UserInfoAlreadyExistsException : ServiceBaseException
{
    public UserInfoAlreadyExistsException(int id) : base($"User with id {id} already exist", ErrorCode.UserAlreadyExists)
    {
    }
}