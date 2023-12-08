using FavoriteBooks.API.Models;

namespace FavoriteBooks.API.Business.Services.Contracts;

public interface IAuthService
{
    //public Task<LoginResponse> LoginAsync(LoginModel model);

    public Task<string?> RegisterAsync(RegisterModel model);
}