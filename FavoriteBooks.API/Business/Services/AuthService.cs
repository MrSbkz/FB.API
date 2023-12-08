using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FavoriteBooks.API.Business.Services.Contracts;
using FavoriteBooks.API.Data.Entities;
using FavoriteBooks.API.Data.Enums;
using FavoriteBooks.API.Exceptions;
using FavoriteBooks.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace FavoriteBooks.API.Business.Services;

public class AuthService(UserManager<User> userManager, IConfiguration configuration)
    : IAuthService
{
    private readonly IConfiguration _configuration = configuration;

    public async Task<string> RegisterAsync(RegisterModel model)
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
            throw new InvalidPasswordException("Password error", result);

        await userManager.AddToRoleAsync(user, Role.User);

        return "User created successfully!";
    }

    public async Task<string> LoginAsync(LoginModel model)
    {
        var user = await userManager.FindByNameAsync(model.UserName);

        if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            return await GetToken(user);
       
        throw new WrongCredentialsException();
    }
    
    private async Task<string> GetToken(User user)
    {
        var userRoles = await userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? string.Empty));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddDays(30),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return  new JwtSecurityTokenHandler().WriteToken(token);
    }
}