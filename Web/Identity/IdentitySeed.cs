using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Web.Identity
{
    public class IdentitySeed
    {
        public static async Task Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {

            var user = new User
            {
                FirstName = configuration["User:AdminUser:FirstName"],
                LastName = configuration["User:AdminUser:LastName"],
                UserName = configuration["User:AdminUser:Username"],
                Email = configuration["User:AdminUser:Email"],
                Image = configuration["User:AdminUser:Image"],
                Profession = configuration["User:AdminUser:Profession"],
                EmailConfirmed = true,
            };
            var role = configuration["User:AdminUser:Role"];
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                await roleManager.CreateAsync(new IdentityRole("user"));
                var result = await userManager.CreateAsync(user, configuration["User:AdminUser:Password"]);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                    await userManager.AddToRoleAsync(user, role);
                }

            }

        }
    }
}
