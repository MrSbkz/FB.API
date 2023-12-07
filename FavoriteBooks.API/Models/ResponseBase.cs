using System.Net;

namespace FavoriteBooks.API.Models;

public class ResponseBase(dynamic data, HttpStatusCode statusCode = HttpStatusCode.OK)
{
    public dynamic Data { get; set; } = data;

    public HttpStatusCode StatusCode { get; set; } = statusCode;
}