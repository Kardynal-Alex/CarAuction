using Auction.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Auction.DAL.EF
{
    /// <summary>
    /// Initializing data at startup
    /// Adding new users and roles
    /// </summary>
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string adminPassword = "admin123";
            string adminName = "Oleksandr";
            string adminSurname = "Kardynal";
            string userEmail = "irakardinal@gmail.com";
            string userPassword = "ira1234";
            string userName = "Ira";
            string userSurname = "Kardynal";
            if (await roleManager.FindByNameAsync(IdentityConstants.Admin) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(IdentityConstants.Admin));
            }
            if (await roleManager.FindByNameAsync(IdentityConstants.User) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(IdentityConstants.User));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    Name = adminName,
                    Surname = adminSurname,
                    Role = IdentityConstants.Admin
                };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, IdentityConstants.Admin);
                }
            }
            if (await userManager.FindByNameAsync(userEmail) == null)
            {
                User user = new User
                {
                    Email = userEmail,
                    UserName = userEmail,
                    Name = userName,
                    Surname = userSurname,
                    Role = IdentityConstants.User
                };
                IdentityResult result = await userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, IdentityConstants.User);
                }
            }
        }
    }
}
