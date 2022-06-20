using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using Anotar.NLog;
using Google.Protobuf.Collections;
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
        string result = String.Empty;
        if (dateTime is not null)
        {
            try
            {
                DateTime o = dateTime.Value;
                result = o.ToString(DateTimeFormat);
            }
            catch (Exception e)
            {
                LogTo.Info("Can not convert DateTime to String");
                LogTo.Error(e.ToString);
            }
        }
            
        return result;
    }

    public static DateTime? ConvertStringToDateTime(string stringDateTime)
    {
        DateTime? result = null;
        try
        {
            result = DateTime.ParseExact(stringDateTime, DateTimeFormat, CultureInfo.InvariantCulture);
        }
        catch (Exception e)
        {
            LogTo.Info("Can not convert String to DateTime");
            LogTo.Error(e.ToString);
        }

        return result;
    }

}