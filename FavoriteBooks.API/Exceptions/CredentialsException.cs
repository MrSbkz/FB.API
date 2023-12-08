using Microsoft.AspNetCore.Identity;

namespace FavoriteBooks.API.Exceptions;

public class CredentialsException(string message, IdentityResult? identityResult) : Exception(message)
{
    public IdentityResult? GetIdentityResult()
    {
        return identityResult;
    }
}