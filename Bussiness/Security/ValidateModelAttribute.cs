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
            var errors = context.ModelState.Root.Errors;
            var exception = new AppException.ValidationFailException();
            int i = 0;
            
            foreach (var modelError in errors)
                exception.Data.Add(i++, modelError.ErrorMessage);

            throw exception;
        }
    }

}