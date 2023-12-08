namespace FavoriteBooks.API.Exceptions;

public class AlreadyExistsException(string message) : Exception(message);