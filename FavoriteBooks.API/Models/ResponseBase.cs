using System.Net;

namespace FavoriteBooks.API.Models;

public class ResponseBase(dynamic? data = null, IList<string>? errors = null, HttpStatusCode statusCode = HttpStatusCode.OK)
{
    public dynamic? Data { get; set; } = data;

    public IList<string>? Errors { get; set; } = errors;

    public HttpStatusCode StatusCode { get; set; } = statusCode;
}