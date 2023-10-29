﻿namespace UserCacheService.Domain.Error;

public enum ErrorCode
{
    Default = 0,
    UserAlreadyExists = 1,
    UserNotFound = 2,
    InvalidUserInfoStatus = 3,
    InternalError = 500,
}