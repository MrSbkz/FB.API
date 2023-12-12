namespace FavoriteBooks.API.Exceptions;

public class NotFoundException(string message) : Exception(message);