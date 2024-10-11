using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QueueManagementSystemAPI.Models;
using System.Text.Json;

namespace QueueManagementSystemAPI.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<App1User> userManager,
            RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<App1User>>(userData);

            var roles = new List<AppRole>
            {
                new AppRole{Name = "Member"},
                new AppRole{Name = "Handler"},
                new AppRole{Name = "Admin"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new App1User
            {
                UserName = "admin",
                Name = "Admin",
                LastName = "Admin",
                Authenticated = true
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Handler" });
        }
    }
}
