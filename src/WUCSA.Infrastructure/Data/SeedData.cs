using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WUCSA.Core.Entities.UserModel;

namespace WUCSA.Infrastructure.Data
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider service)
        {
            await CreateUserRolesAsync(service);
            await AddSuperAdminRoleToSiteOwnerAsync(service);
            await CreateUserAccountAsync(service);
        }

        private static async Task CreateUserRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();

            var superAdminRole = await roleManager.RoleExistsAsync(Role.SuperAdmin.ToString());
            var adminRole = await roleManager.RoleExistsAsync(Role.Admin.ToString());

            if (!superAdminRole)
            {
                try
                {
                    await roleManager.CreateAsync(new UserRole(Role.SuperAdmin));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException?.Message);
                }
                
            }
            if (!adminRole)
            {
                await roleManager.CreateAsync(new UserRole(Role.Admin));
            }
        }

        private static async Task AddSuperAdminRoleToSiteOwnerAsync(IServiceProvider service)
        {
            var userManager = service.GetRequiredService<UserManager<AppUser>>();
            var siteOwner = await userManager.FindByEmailAsync("kudratovsuhrob@gmail.com");

            if (siteOwner == null)
                return;

            var hasSuperAdminRole = await userManager.IsInRoleAsync(siteOwner, Role.SuperAdmin.ToString());

            if (!hasSuperAdminRole)
            {
                await userManager.AddToRoleAsync(siteOwner, Role.SuperAdmin.ToString());
            }
        }

        private static async Task CreateUserAccountAsync(IServiceProvider service)
        {
            var userManager = service.GetRequiredService<UserManager<AppUser>>();
            var config = service.GetRequiredService<IConfiguration>();

            var userAccount = await userManager.FindByNameAsync("Futuricon");
            if (userAccount == null)
            {
                try
                {
                    await userManager.CreateAsync(new AppUser()
                    {
                        UserName = "Futuricon",
                        Email = "kudratovsuhrob@gmail.com",
                        EmailConfirmed = true
                    },
                    config.GetSection("EmailPassword").Value ?? "123456");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException?.Message);
                }
                
            }

            try
            {
                var hasSuperAdminRole = await userManager.IsInRoleAsync(userAccount, Role.SuperAdmin.ToString());

                if (!hasSuperAdminRole)
                {
                    await userManager.AddToRoleAsync(userAccount, Role.SuperAdmin.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
            }
        }
    }
}
