using DataAccess.Entities.Token;

namespace DataAccess.Repository;

public interface IRefreshTokenDao
{
    void InsertRefreshToken(RefreshToken token);

    RefreshToken? FindByToken(string token);
    void UpdateToken(RefreshToken token);
}