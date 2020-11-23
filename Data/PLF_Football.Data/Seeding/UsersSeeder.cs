namespace PLF_Football.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using PLF_Football.Common;
    using PLF_Football.Data.Models;

    internal class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var user = this.CreateOwner();
            await SeedUserInRoleAsync(userManager, user);
        }

        private static async Task SeedUserInRoleAsync(
            UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            var userByEmail = userManager.FindByEmailAsync(GlobalConstants.OwnerEmail);
            if (userByEmail.Result != null)
            {
                return;
            }

            var result = await userManager.CreateAsync(user, GlobalConstants.OwnerPassword);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }

            result = await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }
        }

        private ApplicationUser CreateOwner()
        {
            return new ApplicationUser
            {
                UserName = GlobalConstants.OwnerName,
                Email = GlobalConstants.OwnerEmail,
            };
        }
    }
}
