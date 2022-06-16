using System.Net;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using tutoring_online_be.Controllers.Utils;

namespace tutoring_online_be.Security;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            
            context.Result = new JsonResult(
                new ApiResponse()
                {
                    ResultCode = (int)ResultCode.InvalidParams,
                    ResultMessage = ResultCode.InvalidParams.ToString(),
                    Data = context.ModelState.Root.Errors
                }
            ) { StatusCode = StatusCodes.Status400BadRequest };
        }
    }

}