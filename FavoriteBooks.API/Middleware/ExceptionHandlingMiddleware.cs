using System.Net;
using FavoriteBooks.API.Exceptions;
using FavoriteBooks.API.Models;

namespace FavoriteBooks.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = exception switch
        {
            NotFoundException _ => new ResponseBase(null, new List<string>{ exception.Message }, HttpStatusCode.NotFound),
            AlreadyExistsException _ => new ResponseBase(null, new List<string>{ exception.Message }, HttpStatusCode.Conflict),
            InvalidPasswordException _ => new ResponseBase(null, GetCredentialsErrorList(exception), HttpStatusCode.Forbidden),
            WrongCredentialsException _ => new ResponseBase(null, new List<string> { exception.Message }, HttpStatusCode.Unauthorized),
            _ => new ResponseBase(null, new List<string>{ exception.Message }, HttpStatusCode.InternalServerError)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }

    private static IList<string> GetCredentialsErrorList(Exception exception)
    {
        var ex = (InvalidPasswordException)exception;
        var errors = new List<string> { exception.Message };
        var result = ex.GetIdentityResult();

        if(result != null)
            errors.AddRange(result.Errors.Select(x => x.Description).ToList());

        return errors;
    }
}