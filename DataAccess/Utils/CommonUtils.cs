using System.Globalization;
using MySql.Data.Types;

namespace DataAccess.Utils;

public class CommonUtils
{
    public const string DateTimeFormat = "dd-MM-yyyy HH:mm:ss";

    public static string ConvertMySqlDateTimeToString(MySqlDateTime? dateTime)
    {
        if (dateTime is null)
            return string.Empty;

        return dateTime.ToString();
    }
    
    public static string ConvertDateTimeToString(DateTime? dateTime)
    {
        if (dateTime is null)
            return string.Empty;
        
        
        DateTime o = dateTime.Value;
        return o.ToString(DateTimeFormat);
    }

    public static DateTime ConvertStringToDateTime(string stringDateTime)
    {
            return DateTime.ParseExact(stringDateTime, DateTimeFormat, CultureInfo.InvariantCulture);

    }
}