using DataAccess.Entities.Token;
using DataAccess.Models.Token;

namespace tutoring_online_be.Services;

public interface IAuthenticationService
{
    void InsertRefreshToken(RefreshToken token);

    RefreshTokenDto? FindByToken(string token);

    void UpdateToken(RefreshToken token);

    void FindUserByFirebaseUid(string uid);
}