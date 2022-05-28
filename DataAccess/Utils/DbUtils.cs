using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace DataAccess.Utils;

public static class DbUtils
{
    private const string ConnectionString = "server=tutoring-online.mysql.database.azure.com;user=dbadmin;database=OTA;password=tutor@1369";
    private const string MysqlServer = "tutoring-online.mysql.database.azure.com";
    private const string MysqlUser = "dbadmin";
    private const string MysqlPassword = "tutor@1369";
    private const string Database = "OTA";

    public static MySqlConnection GetMySqlDbConnection(ILogger logger)
    {
        var MySqlStringBuilder = new MySqlConnectionStringBuilder();
        MySqlStringBuilder["Server"] = MysqlServer;
        MySqlStringBuilder["Database"] = Database;
        MySqlStringBuilder["User Id"] = MysqlUser;
        MySqlStringBuilder["Password"] = MysqlPassword;

        var mysqlConnectionString = MySqlStringBuilder.ToString();
        logger.LogInformation(LogUtils.CreateLogMessage($"Got DB connection for {Database} at {MysqlServer}"));

        return new MySqlConnection(mysqlConnectionString);
    }

    public static void CloseMySqlDbConnection(ILogger logger)
    {
        logger.LogInformation(LogUtils.CreateLogMessage($"Closed connection for {Database} at {MysqlServer}"));
    }

    public static MySqlCommand CreateMySqlCommand(string commandText ,ILogger logger, MySqlConnection connection)
    {
        var command = new MySqlCommand(commandText, connection);
        
        logger.LogInformation(LogUtils.CreateLogMessage($"Query : {commandText}"));

        return command;
    }
}