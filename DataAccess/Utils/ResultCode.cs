namespace DataAccess.Utils;

public enum ResultCode
{
    Success = 0,
    UserAlreadySignup = 20,
    Unknown = 30,
    InvalidParams = 92,
    TokenNotMatch = 93,
    RefreshTokenHasBeenRevoked = 94,
    RefreshTokenHasBeenUsed = 95,
    UserRoleNotFound = 96,
    AccessTokenExpired = 97,
    InvalidToken = 98,
    SystemError = 99,
    InvalidUser = 100,
    Unauthorized = 401,
    UserAlreadyCreated = 909

}