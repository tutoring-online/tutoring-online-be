﻿namespace tutoring_online_be.Controllers.Utils;

public enum ResultCode
{
    Success = 0,
    Unknown = 1,
    TokenNotMatch = 93,
    RefreshTokenHasBeenRevoked = 94,
    RefreshTokenHasBeenUsed = 95,
    RefreshTokenNotExist = 96,
    AccessTokenNotExpired = 97,
    InvalidToken = 98,
    SystemError = 99,
    InvalidUser = 100
    
}