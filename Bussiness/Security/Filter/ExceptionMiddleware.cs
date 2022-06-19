using System.Net;
using Anotar.NLog;
using DataAccess.Models;
using tutoring_online_be.Controllers.Utils;

namespace tutoring_online_be.Security.Filter;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, (int)HttpStatusCode.InternalServerError, ResultCode.Unknown);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode, ResultCode resultCode)
    {
        LogTo.Error($"Something went wrong: {exception}");
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        var response = Newtonsoft.Json.JsonConvert.SerializeObject(new ApiResponse()
        {
            ResultCode = (int)resultCode,
            ResultMessage = resultCode.ToString(),
            Data = new List<object>()
        });
        await context.Response.WriteAsync(response);
    }
}