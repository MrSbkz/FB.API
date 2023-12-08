namespace FavoriteBooks.API.Models;

public class RegisterModel
{
    public string UserName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
        
    public string Password { get; set; } = string.Empty;
}