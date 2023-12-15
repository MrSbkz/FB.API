using System.Net;
using FavoriteBooks.API.Exceptions;
using FavoriteBooks.API.Models;

namespace FavoriteBooks.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, logger);
        }
    }
    
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ExceptionHandlingMiddleware> logger)
    {
        var response = exception switch
        {
            NotFoundException _ => new ResponseBase(null, new List<string>{ exception.Message }, HttpStatusCode.NotFound),
            AlreadyExistsException _ => new ResponseBase(null, new List<string>{ exception.Message }, HttpStatusCode.Conflict),
            InvalidPasswordException invalidPasswordException => new ResponseBase(null, invalidPasswordException.GetErrors(), HttpStatusCode.Forbidden),
            WrongCredentialsException _ => new ResponseBase(null, new List<string> { exception.Message }, HttpStatusCode.Unauthorized),
            _ => GetUnhandledExceptionResult(exception, logger)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }

    private static ResponseBase GetUnhandledExceptionResult(Exception exception, ILogger<ExceptionHandlingMiddleware> logger)
    {
        logger.LogError(exception, "An unexpected error occured");

        return new ResponseBase(null, new List<string> { exception.Message }, HttpStatusCode.InternalServerError);
    }
}