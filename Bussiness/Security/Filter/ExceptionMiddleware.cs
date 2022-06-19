using System.Collections;
using System.Net;
using Anotar.NLog;
using DataAccess.Models;
using tutoring_online_be.Controllers.Utils;

namespace tutoring_online_be.Security.Filter;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var errors = new List<string?>();
        try
        {
            await next(context);
        }
        catch (AppException.ValidationFailException ex)
        {
            foreach (DictionaryEntry o in ex.Data)
            {
                errors.Add(o.Value is not null ? o.Value.ToString() : "");
            }

            await HandleExceptionAsync(context, ex, (int)HttpStatusCode.BadRequest, ResultCode.InvalidParams, errors);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, (int)HttpStatusCode.InternalServerError, ResultCode.Unknown, errors);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception appException, int statusCode, ResultCode resultCode, object data)
    {
        LogTo.Error($"Something went wrong: {appException}");
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        var response = Newtonsoft.Json.JsonConvert.SerializeObject(new ApiResponse()
        {
            ResultCode = (int)resultCode,
            ResultMessage = resultCode.ToString(),
            Data = data
        });
        await context.Response.WriteAsync(response);
    }
}