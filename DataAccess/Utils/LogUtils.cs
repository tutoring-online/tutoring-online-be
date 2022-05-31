using Microsoft.Extensions.Logging;

namespace DataAccess.Utils;

public static class LogUtils
{
    public static string CreateLogMessage(string? message)
    {
        if (message is null)
            message = "";

        return DateTime.Now + " ============= " + message;
    }
    
}