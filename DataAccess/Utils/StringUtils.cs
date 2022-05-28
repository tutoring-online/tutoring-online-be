namespace DataAccess.Utils;

public class StringUtils
{
    public static string? NullToEmpty(string? s)
    {
        if(s is null)
            return "";

        return s;
    }
}