using System.Text;
using Anotar.NLog;

namespace tutoring_online_be.Security.Filter;

public class RequestResponseHandlerMiddleware : IMiddleware
{
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Request.EnableBuffering();
        var requestBodyAsText = await new StreamReader(context.Request.Body).ReadToEndAsync();
        //  Set the position of the stream to 0 to enable rereading
        context.Request.Body.Position = 0;

        LogTo.Info(
            $"\nProtocol: {context.Request.Protocol}" +
                   $"\nMethod: {context.Request.Method}" +
                   $"\nPath: {context.Request.Path}" +
                   $"\nQuery string: {context.Request.QueryString}" +
                   $"\nBody: {requestBodyAsText}" 
            );

        var originalBody = context.Response.Body;
        using var newBody = new MemoryStream();
        context.Response.Body = newBody;
        
        await next(context);
        
        newBody.Seek(0, SeekOrigin.Begin);
        var responseBodyAsText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        LogTo.Info(
                $"\nProtocol: {context.Request.Protocol}" +
                $"\nMethod: {context.Request.Method}" +
                $"\nPath: {context.Request.Path}" +
                $"\nQuery string: {context.Request.QueryString}" +
                $"\nResponse Body: {responseBodyAsText}" 
            );
            
        newBody.Seek(0, SeekOrigin.Begin);
        await newBody.CopyToAsync(originalBody);
    }
}