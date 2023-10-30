using UserCacheService.Domain.Exceptions;

namespace UserCacheService.Domain.UserInfo;

public enum UserInfoStatus
{
    New = 1,
    Active = 2,
    Blocked = 3,
    Deleted = 4,
}

public static class UserInfoStatusHelper
{
    public static UserInfoStatus TryParseNewStatusOrThrow(string newStatus) => newStatus switch
    {
        nameof(UserInfoStatus.New) => UserInfoStatus.New,
        nameof(UserInfoStatus.Active) => UserInfoStatus.Active,
        nameof(UserInfoStatus.Deleted) => UserInfoStatus.Deleted,
        nameof(UserInfoStatus.Blocked) => UserInfoStatus.Blocked,
        var _ => throw new InvalidUserInfoStatusException(newStatus),
    };
}