using Anotar.NLog;
using DataAccess.Models.Authentication;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Extensions;
using tutoring_online_be.Controllers.Utils;

namespace tutoring_online_be.Security;

public class AuthorizeAttribute : System.Attribute, IAuthorizationFilter
{
    private readonly IList<Role> roles;

    public AuthorizeAttribute(params Role[]? roles)
    {
        this.roles = roles ?? new Role[] { };
    }
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        LogTo.Info("\nStart authorization filer");
        var headers = context.HttpContext.Request.Headers;
        var role = "";

        if (headers.ContainsKey("TEST"))
        {
            if (headers["TEST"].ToString().ToLower().Equals("true"))
            {
                LogTo.Info("\nIs testing - skip authorize");
                return;
            }
        }
        
        if (headers.ContainsKey("Authorization"))
        {
            try
            {
                LogTo.Info("\nCall Firebase to verify token");
                var decodedToken = FirebaseAuth.DefaultInstance
                    .VerifyIdTokenAsync(headers["Authorization"]).Result;

                var userRecord = FirebaseAuth.DefaultInstance.GetUserAsync(decodedToken.Uid).Result;
                var customClaims = userRecord.CustomClaims;
                if (customClaims.ContainsKey("role"))
                {
                    role = customClaims["role"].ToString();
                }
            }
            catch (AggregateException e)
            {
                LogTo.Info(e.ToString);
                context.Result = new JsonResult(new
                {
                    ResultMessage = ResultCode.InvalidToken.ToString(),
                    ResultCode = (int) ResultCode.InvalidToken
                }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }

        
        if (!roles.Any(t => t.ToString().Equals(role)))
        {
            // not logged in or role not authorized
            context.Result = new JsonResult(new
            {
                ResultMessage = ResultCode.Unauthorized.ToString(),
                ResultCode = (int) ResultCode.Unauthorized
            }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
        
    }
}