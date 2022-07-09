using System.Data;
using System.Runtime.CompilerServices;
using Anotar.NLog;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace DataAccess.Utils;

public static class DbUtils
{
    private const string MysqlServer = "35.198.220.120";
    private const string MysqlUser = "admin";
    private const string MysqlPassword = "123456";
    private const string Database = "OTA";

    public static MySqlConnection GetMySqlDbConnection()
    {
        var MySqlStringBuilder = new MySqlConnectionStringBuilder();
        MySqlStringBuilder["Server"] = MysqlServer;
        MySqlStringBuilder["Database"] = Database;
        MySqlStringBuilder["User Id"] = MysqlUser;
        MySqlStringBuilder["Password"] = MysqlPassword;

        var mysqlConnectionString = MySqlStringBuilder.ToString();
        LogTo.Info($"Got DB connection for {Database} at {MysqlServer}");

        return new MySqlConnection(mysqlConnectionString);
    }

    public static void CloseMySqlDbConnection()
    {
        LogTo.Info($"Closed connection for {Database} at {MysqlServer}");
    }

    public static MySqlCommand CreateMySqlCommand(string commandText , MySqlConnection connection)
    {
        var command = new MySqlCommand(commandText, connection);
        
        LogTo.Info($"Query : {commandText}");

        return command;
    }
    
    public static string SafeGetString(this MySqlDataReader reader, string colName)
    {
        if(!reader.IsDBNull(colName))
            return reader.GetString(colName);
        return string.Empty;
    }
    
    public static string SafeGetString(this MySqlDataReader reader, int colIndex)
    {
        if(!reader.IsDBNull(colIndex))
            return reader.GetString(colIndex);
        return string.Empty;
    }
    
    public static int SafeGetInt16(this MySqlDataReader reader, string colName)
    {
        if(!reader.IsDBNull(colName))
            return reader.GetInt16(colName);
        return 0;
    }
    
    public static int SafeGetInt16(this MySqlDataReader reader, int colIndex)
    {
        if(!reader.IsDBNull(colIndex))
            return reader.GetInt16(colIndex);
        return 0;
    }

    public static double SafeGetDouble(this MySqlDataReader reader, string colName)
    {
        if (!reader.IsDBNull(colName))
            return reader.GetDouble(colName);
        return 0;
    }

    public static double SafeGetDouble(this MySqlDataReader reader, int colIndex)
    {
        if (!reader.IsDBNull(colIndex))
            return reader.GetDouble(colIndex);
        return 0;
    }
    public static MySqlDateTime? SafeGetMySqlDateTime(this MySqlDataReader reader, string colName)
    {
        if(!reader.IsDBNull(colName))
            return reader.GetMySqlDateTime(colName);
        return null;
    }
    
    public static MySqlDateTime? SafeGetMySqlDateTime(this MySqlDataReader reader, int colIndex)
    {
        if(!reader.IsDBNull(colIndex))
            return reader.GetMySqlDateTime(colIndex);
        return null;
    }
    
    public static DateTime? SafeGetDateTime(this MySqlDataReader reader, string colName)
    {
        if(!reader.IsDBNull(colName))
            return reader.GetDateTime(colName);
        return null;
    }
    
    public static DateTime? SafeGetDateTime(this MySqlDataReader reader, int colIndex)
    {
        if(!reader.IsDBNull(colIndex))
            return reader.GetDateTime(colIndex);
        return null;
    }
    
    public static Boolean? SafeGetBoolean(this MySqlDataReader reader, int colIndex)
    {
        if(!reader.IsDBNull(colIndex))
            return reader.GetBoolean(colIndex);
        return null;
    }
    
    public static Boolean? SafeGetBoolean(this MySqlDataReader reader, string colName)
    {
        if(!reader.IsDBNull(colName))
            return reader.GetBoolean(colName);
        return null;
    }
}