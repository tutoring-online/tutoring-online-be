using System.Text;
using Anotar.NLog;

namespace tutoring_online_be.Security.Filter;

public class RequestResponseHandlerMiddleware : IMiddleware
{
    private readonly string xRequestId = "X-Request-Id";
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var requestId = "";
        context.Request.EnableBuffering();
        var requestBodyAsText = await new StreamReader(context.Request.Body).ReadToEndAsync();
        //  Set the position of the stream to 0 to enable rereading
        context.Request.Body.Position = 0;

        var headers = context.Request.Headers;

        
        if (headers.ContainsKey(xRequestId))
        {
            requestId = headers[xRequestId].ToString();
        }
        
        LogTo.Info(
            $"\nProtocol: {context.Request.Protocol}" +
                   $"\nMethod: {context.Request.Method}" +
                   $"\nPath: {context.Request.Path}" +
                   $"\nQuery string: {context.Request.QueryString}" +
                   $"\nRequest-id: {requestId}" +
                   $"\nBody: {requestBodyAsText}" 
            );

        var originalBody = context.Response.Body;
        using var newBody = new MemoryStream();
        context.Response.Body = newBody;
        context.Response.Headers.Add(xRequestId, requestId);
        
        await next(context);
        
        newBody.Seek(0, SeekOrigin.Begin);
        var responseBodyAsText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        LogTo.Info(
            $"\nProtocol: {context.Request.Protocol}" +
                   $"\nMethod: {context.Request.Method}" +
                   $"\nPath: {context.Request.Path}" +
                   $"\nQuery string: {context.Request.QueryString}" +
                   $"\nRequest-id: {requestId}" +
                   $"\nResponse Body: {responseBodyAsText}" 
            );
            
        newBody.Seek(0, SeekOrigin.Begin);
        await newBody.CopyToAsync(originalBody);
        
        Console.WriteLine("Start save log to db");
        
        
        Console.WriteLine("End save log to db");
        
        
    }
}