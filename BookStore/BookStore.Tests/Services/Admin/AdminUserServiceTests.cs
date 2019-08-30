using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Admin;
using BookStore.Services.Admin.Models.Users;
using BookStore.Services.Mapping;
using BookStore.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Tests.Services.Admin
{
    public class AdminUserServiceTests
    {
        private IAdminUserService userService;

        public AdminUserServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        private List<BookStoreUser> GetTestData()
        {
            return new List<BookStoreUser>()
            {
                new BookStoreUser
                {
                    UserName = "user1",
                    Email = "user1@gmail.com",
                },
                new BookStoreUser
                {
                   UserName = "user2",
                   Email = "user2@gmail.com",
                }
            };
        }

        private async Task SeedData(BookStoreDbContext context)
        {
            context.AddRange(GetTestData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllUsers_WithData_ShouldReturnAllUsers()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.userService = new AdminUserService(context);
      
            List<AdminUserListingServiceModel> expectedData = GetTestData().To<AdminUserListingServiceModel>().ToList();
            List<AdminUserListingServiceModel> actualData = await this.userService.GetAllUsers().ToListAsync();

            Assert.Equal(expectedData.Count, actualData.Count);

            foreach (var actualUser in actualData)
            {
                Assert.True(expectedData.Any(user => actualUser.Username == user.Username
                && actualUser.Email == user.Email),"AdminUserService GetAllUsers() does not work properly");
            }

            for (int i = 0; i < expectedData.Count; i++)
            {
                var expectedEntry = expectedData[i];
                var actualEntry = actualData[i];

                Assert.True(expectedEntry.Username == actualEntry.Username, "Username is not returned properly.");
                Assert.True(expectedEntry.Email == actualEntry.Email, "Email is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllUsers_WithoutData_ShouldReturnEmptyList()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            this.userService = new AdminUserService(context);

            List<AdminUserListingServiceModel> actualData = await this.userService.GetAllUsers().ToListAsync();

            Assert.Empty(actualData);
        }
    }
}
