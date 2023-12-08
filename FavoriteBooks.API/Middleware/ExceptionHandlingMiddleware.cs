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
            _ => new ResponseBase(null, new List<string>{ exception.Message }, HttpStatusCode.InternalServerError)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}