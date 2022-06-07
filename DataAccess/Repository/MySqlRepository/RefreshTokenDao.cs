using Anotar.NLog;
using DataAccess.Entities.Token;
using DataAccess.Utils;
using MySql.Data.MySqlClient;

namespace DataAccess.Repository.MySqlRepository;

public class RefreshTokenDao : IRefreshTokenDao
{
    public void InsertRefreshToken(RefreshToken token)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            
            IEnumerable<RefreshToken> tokens = new[]
            {
                token
            };

            var command = MySqlUtils.CreateInsertStatement(tokens, connection);
            
            command.ExecuteNonQuery();
            
        }
        catch (MySqlException e)
        {
            LogTo.Error(e.ToString());
        }
        catch (Exception e)
        {
            LogTo.Error(e.ToString());
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }
    }

    public RefreshToken? FindByToken(string token)
    {
        RefreshToken? refreshToken = null;
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@token";

            var selectStatement = "Select Id, JwtId, UserId, UserRole, IsUsed, IsRevoked, IssuedAt, ExpiredAt, Token";
            var fromStatement = "From RefreshToken";
            var whereStatement = $"Where Token = {param1}";
            var query = selectStatement + " " + fromStatement + " " + whereStatement; 
                                  
            using var command = DbUtils.CreateMySqlCommand(query, connection);
            command.CommandText = query;

            command.Parameters.Add(param1, MySqlDbType.VarChar).Value = token;
            command.Prepare();
            
            foreach (MySqlParameter commandParameter in command.Parameters)
            {
                LogTo.Info($"Param {commandParameter}: {commandParameter.Value}");
            }

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                refreshToken = new RefreshToken()
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    JwtId = DbUtils.SafeGetString(reader, "JwtId"),
                    UserId = DbUtils.SafeGetString(reader, "UserId"),
                    UserRole = DbUtils.SafeGetString(reader, "UserRole"),
                    IsUsed = DbUtils.SafeGetBoolean(reader, "IsUsed"),
                    IsRevoked = DbUtils.SafeGetBoolean(reader, "IsRevoked"),
                    Token = DbUtils.SafeGetString(reader, "Token"),
                    IssuedAt = DbUtils.SafeGetDateTime(reader, "IssuedAt"),
                    ExpiredAt = DbUtils.SafeGetDateTime(reader, "ExpiredAt")
                };
            }
        }
        catch (MySqlException e)
        {
            LogTo.Error(e.ToString());
        }
        catch (Exception e)
        {
            LogTo.Error(e.ToString());
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }

        return refreshToken;
    }

    public void UpdateToken(RefreshToken token)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@isUsed";
            var param2 = "@isRevoked";
            var param3 = "@id";

            var updateStatement = "Update RefreshToken";
            var setStatement1 = $"Set IsUsed = {param1}";
            var setStatement2 = $"IsRevoked = {param2}";
            var whereStatement = $"Where Id = {param3}";
            var query = updateStatement + " " + setStatement1 + "," + setStatement2 + " " + whereStatement;
                                  
            using var command = DbUtils.CreateMySqlCommand(query, connection);
            command.CommandText = query;

            command.Parameters.Add(param1, MySqlDbType.Bit).Value = token.IsUsed;
            command.Parameters.Add(param2, MySqlDbType.Bit).Value = token.IsRevoked;
            command.Parameters.Add(param3, MySqlDbType.VarChar).Value = token.Id;
            command.Prepare();
            
            foreach (MySqlParameter commandParameter in command.Parameters)
            {
                LogTo.Info($"Param {commandParameter}: {commandParameter.Value}");
            }

            command.ExecuteNonQuery();
        }
        catch (MySqlException e)
        {
            LogTo.Error(e.ToString());
        }
        catch (Exception e)
        {
            LogTo.Error(e.ToString());
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }
    }
}