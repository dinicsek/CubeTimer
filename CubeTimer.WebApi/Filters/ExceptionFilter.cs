using System.Net;
using CubeTimer.WebApi.Execption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CubeTimer.WebApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (!context.ExceptionHandled)
        {
            var exception = context.Exception;


            int statusCode;


            switch (true)
            {
                case bool _ when exception is ControllerValidationException:
                    context.Result = new BadRequestObjectResult(new ValidationProblemDetails(new Dictionary<string, string[]>
                    {
                        { ((ControllerValidationException)exception).Field, ((ControllerValidationException)exception).Errors }
                    }));
                    break;


                default:
                    break;
            }
        }
    }
}