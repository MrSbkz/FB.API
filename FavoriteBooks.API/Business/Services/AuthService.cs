using FavoriteBooks.API.Business.Services.Contracts;
using FavoriteBooks.API.Data.Entities;
using FavoriteBooks.API.Exceptions;
using FavoriteBooks.API.Models;
using Microsoft.AspNetCore.Identity;

namespace FavoriteBooks.API.Business.Services;

public class AuthService(UserManager<User> userManager, IConfiguration configuration)
    : IAuthService
{
    private readonly IConfiguration _configuration = configuration;

    public async Task<string?> RegisterAsync(RegisterModel model)
    {
        var existingUser = await userManager.FindByNameAsync(model.UserName);

        if (existingUser != null)
            throw new AlreadyExistsException("User already exists");

        var user = new User
        {
            UserName = model.UserName,
            FirstName = model.FirstName,
            LastName = model.LastName,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        var result = await userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            throw new CredentialsException("Credential error", result);

        await userManager.AddToRoleAsync(user, "user");

        return "User created successfully!";
    }
}