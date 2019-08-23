using BookStore.Data;
using BookStore.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<BookStoreDbContext>().Database.Migrate();

                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<BookStoreUser>>();

                Task
                    .Run(async () =>
                    {
                        var adminName = WebConstants.AdministratorRole; 

                        var roleExists = await roleManager.RoleExistsAsync(adminName);

                        if (!roleExists)
                        {
                            await roleManager.CreateAsync(new IdentityRole
                            {
                                Name = adminName
                            });
                        }


                        var adminUser = await userManager.FindByNameAsync(adminName);

                        if (adminUser == null)
                        {
                            adminUser = new BookStoreUser
                            {
                                UserName = "admin",
                                Email = "admin@gmail.com",
                                ShoppingCart = new ShoppingCart()
                            };

                            var result = await userManager.CreateAsync(adminUser, "123456");

                            if (result.Succeeded)
                            {
                                await userManager.AddToRoleAsync(adminUser, WebConstants.AdministratorRole);
                            }
                        }
                    })
                    .GetAwaiter()
                    .GetResult();
            }
            return app;
        }
    }
}
