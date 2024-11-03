using System.Net;
using FluentValidation;
using SkillUpHub.Command.Application.Exceptions;

namespace SkillUpHub.Profile.API.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch(Exception ex)
        {
            ExceptionHandler(context, ex);
        }
    }

    private void ExceptionHandler(HttpContext context, Exception exception)
    {
        var type = exception.GetType();
        
        if (type == typeof(ValidationException) || type == typeof(HandledException))
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            context.Response.WriteAsJsonAsync(exception.Message);
        }
        else
        {
            Console.WriteLine(exception.Message);
            //Здесь будет логирование
        }
    }
}