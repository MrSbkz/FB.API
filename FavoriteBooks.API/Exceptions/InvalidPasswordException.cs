using Microsoft.AspNetCore.Identity;

namespace FavoriteBooks.API.Exceptions;

public class InvalidPasswordException(string message, IdentityResult? identityResult) : Exception(message)
{
    public IList<string> GetErrors()
    {
        var errors = new List<string> { Message };

        if(identityResult != null)
            errors.AddRange(identityResult.Errors.Select(x => x.Description).ToList());

        return errors;
    }
}