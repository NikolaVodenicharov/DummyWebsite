using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Tyres.Data.Models;
using Tyres.Data.Models.Orders;

namespace Tyres.Web.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder CreateDefaultRoles(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                var roles = new[]
                {
                    Constants.Administrator,
                    Constants.Worker,
                    Constants.LoggedUser
                };

                foreach (var role in roles)
                {
                    CreateRole(role, roleManager);
                }
            }

            return app;
        }
        private static void CreateRole(string roleName, RoleManager<IdentityRole> roleManager)
        {
            Task
                .Run(async () =>
                {
                    var isRoleExist = await roleManager.RoleExistsAsync(roleName);

                    if (!isRoleExist)
                    {
                        var role = new IdentityRole(roleName);
                        await roleManager.CreateAsync(role);
                    }
                })
                .Wait();
        }

        public static IApplicationBuilder CreateDefaultAdministrator(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                Task
                    .Run(async () =>
                    {
                        var user = await userManager.FindByNameAsync(Constants.Administrator);

                        var isAdminExist = user != null;
                        if (isAdminExist)
                        {
                            return;
                        }

                        var isRoleExist = await roleManager.RoleExistsAsync(Constants.Administrator);
                        if (!isRoleExist)
                        {
                            throw new TaskCanceledException($"Can not create {Constants.Administrator} because the role does not exist.");
                        }

                        user = new User
                        {
                            UserName = $"{Constants.Administrator}@gmail.com",
                            Email = $"{Constants.Administrator}@gmail.com",
                            PhoneNumber = "+123456123456",
                            FirstName = "John",
                            LastName = "Doe",
                            DeliveryAddress = "None"
                        };

                        var result = await userManager.CreateAsync(user, "admin123");
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, Constants.Administrator);
                        }
                    })
                    .Wait();
            }

            return app;
        }
    }
}
