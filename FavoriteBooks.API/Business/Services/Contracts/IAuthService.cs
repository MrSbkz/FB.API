using FavoriteBooks.API.Models;

namespace FavoriteBooks.API.Business.Services.Contracts;

public interface IAuthService
{
    public Task<string> RegisterAsync(RegisterModel model);
    
    public Task<string> LoginAsync(LoginModel model);
}