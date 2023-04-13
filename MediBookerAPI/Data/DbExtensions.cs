using MediBookerAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace MediBookerAPI.Data
{
    public static class DbExtensions
    {
        public static async void CreateRoles(this IServiceCollection serviceCollection)
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var userRole = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userModerator = serviceProvider.GetRequiredService<UserManager<User>>();

            User moderator = new User();
            moderator.UserName = "Moderator";
            moderator.Email = "Moderator@example.com";
            await userModerator.CreateAsync(moderator, "Moderator123!");

            await userRole.CreateAsync(new IdentityRole("Moderator"));
            await userRole.CreateAsync(new IdentityRole("Doctor"));
            await userRole.CreateAsync(new IdentityRole("Worker"));

            await userModerator.AddToRoleAsync(moderator, "Moderator");
        }    
    }
}
