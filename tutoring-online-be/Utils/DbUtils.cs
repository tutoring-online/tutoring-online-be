namespace tutoring_online_be.Utils;

public class DbUtils
{
    private const string ConnectionString = "server=tutoring-online.mysql.database.azure.com;user=dbadmin;database=OTA;password=tutor@1369";

    public static string GetDbConnection()
    {
        return ConnectionString;
    }    
}