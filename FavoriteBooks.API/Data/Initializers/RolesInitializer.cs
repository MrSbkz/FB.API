using FavoriteBooks.API.Data.Enums;
using Microsoft.AspNetCore.Identity;

namespace FavoriteBooks.API.Data.Initializers;

public static class RolesInitializer
{
    public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
    {
        if (await roleManager.FindByNameAsync(Role.Admin) == null)
        {
            await roleManager.CreateAsync(new IdentityRole(Role.Admin));
        }

        if (await roleManager.FindByNameAsync(Role.User) == null)
        {
            await roleManager.CreateAsync(new IdentityRole(Role.User));
        }

        if (await roleManager.FindByNameAsync(Role.Moderator) == null)
        {
            await roleManager.CreateAsync(new IdentityRole(Role.Moderator));
        }
    }
}