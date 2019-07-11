using BookStore.Data;
using BookStore.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        //public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        //{
        //    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        //    {
        //        serviceScope.ServiceProvider.GetService<BookStoreDbContext>().Database.Migrate();

        //        var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
        //        var userManager = serviceScope.ServiceProvider.GetService<UserManager<BookStoreUser>>();

        //        Task
        //            .Run(async () =>
        //            {
        //                // Seed Roles TODO
        //                var roles = new[]
        //                {
        //                    WebConstants.AdministratorRole,
        //                };

        //                foreach (var role in roles)
        //                {
        //                    var roleExists = await roleManager.RoleExistsAsync(role);

        //                    if (!roleExists)
        //                    {
        //                        await roleManager.CreateAsync(new IdentityRole
        //                        {
        //                            Name = role
        //                        });
        //                    }
        //                }

        //                // Seed Admin User

        //                if (!userManager.Users.Any())
        //                {
        //                    // Create Admin User
        //                    var adminUser = new BookStoreUser
        //                    {
        //                        UserName = AdminUsername, // change Register action as well
        //                        Email = AdminEmail,
        //                        // custom properties:
        //                        Name = WebConstants.AdministratorRole // Add Name to User
        //                    };

        //                    var result = await userManager.CreateAsync(adminUser, AdminPassword);

        //                    // Add User to Role
        //                    if (result.Succeeded)
        //                    {
        //                        await userManager.AddToRoleAsync(adminUser, WebConstants.AdministratorRole);
        //                    }
        //                }
        //                else
        //                {
        //                    // Add User to Role
        //                    await userManager.AddToRoleAsync(adminUser, WebConstants.AdministratorRole);
        //                }
        //            })
        //            .GetAwaiter()
        //            .GetResult();
        //    }

        //    return app;
        //}
    }
}
