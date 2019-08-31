using BookStore.Data;
using BookStore.Domain;
using BookStore.Models;
using BookStore.Services;
using BookStore.Services.Admin;
using BookStore.Services.Admin.Models.Users;
using BookStore.Services.Mapping;
using BookStore.Web.Infrastructure.Extensions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BookStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BookStoreDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<BookStoreUser,IdentityRole>()
                .AddEntityFrameworkStores<BookStoreDbContext>()
                .AddDefaultTokenProviders();

            Account cloudinaryCredentials = new Account(
          this.Configuration["Cloudinary:CloudName"],
          this.Configuration["Cloudinary:ApiKey"],
          this.Configuration["Cloudinary:ApiSecret"]);

            Cloudinary cloudinaryUtility = new Cloudinary(cloudinaryCredentials);

            services.AddSingleton(cloudinaryUtility);

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;

                options.User.RequireUniqueEmail = true;
            });

            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IAdminUserService, AdminUserService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAdminCategoryService, AdminCategoryService>();
            services.AddTransient<IAdminPublisherService, AdminPublisherService>();
            services.AddTransient<IAdminAuthorService, AdminAuthorService>();
            services.AddTransient<IAdminBookService, AdminBookService>();
            services.AddTransient<IAdminReviewService, AdminReviewService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<IShoppingCartService, ShoppingCartService>();
            services.AddTransient<IOrderService, OrderService>();

            services.AddMvc(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = $"/Identity/Account/Login";
                    options.LogoutPath = $"/Identity/Account/Logout";
                    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).GetTypeInfo().Assembly,
                typeof(AdminUserListingServiceModel).GetTypeInfo().Assembly);

            app.UseDatabaseMigration();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
