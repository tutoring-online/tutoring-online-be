using DataAccess.Entities.Token;
using DataAccess.Models.Token;
using DataAccess.Repository;
using DataAccess.Utils;

namespace tutoring_online_be.Services.V1;

public class AuthenticationServiceV1 : IAuthenticationService
{
    private IRefreshTokenDao refreshTokenDao;
    
    public AuthenticationServiceV1(IRefreshTokenDao refreshTokenDao)
    {
        this.refreshTokenDao = refreshTokenDao;
    }

    public void InsertRefreshToken(RefreshToken token)
    {
        refreshTokenDao.InsertRefreshToken(token);
    }

    public RefreshTokenDto FindByToken(string token)
    {
        return refreshTokenDao.FindByToken(token).AsDto();
    }

    public void UpdateToken(RefreshToken token)
    {
        refreshTokenDao.UpdateToken(token);
    }
}