namespace tutoring_online_be.Utils;

public class StringUtils
{
    public static string? NullToEmpty(string? s)
    {
        if(s is null)
            return "";

        return s;
    }
}