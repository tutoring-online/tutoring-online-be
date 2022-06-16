namespace tutoring_online_be.Controllers.Utils;

public enum ResultCode
{
    Success = 0,
    Unknown = 1,
    InvalidParams = 92,
    TokenNotMatch = 93,
    RefreshTokenHasBeenRevoked = 94,
    RefreshTokenHasBeenUsed = 95,
    UserRoleNotFound = 96,
    AccessTokenExpired = 97,
    InvalidToken = 98,
    SystemError = 99,
    InvalidUser = 100,
    Unauthorized = 401

}