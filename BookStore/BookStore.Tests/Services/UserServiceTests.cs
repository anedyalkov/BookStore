using BookStore.Data;
using BookStore.Domain;
using BookStore.Services;
using BookStore.Tests.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Tests.Services
{
    public class UserServiceTests
    {
        private IUserService userService;

        public UserServiceTests()
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
        public async Task GetByUsernameAsync_ShouldReturnUserByUsername()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            var username = "user1";
            var userStore = new Mock<IUserStore<BookStoreUser>>();
            var userManager = new Mock<UserManager<BookStoreUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.FindByNameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.userService = new UserService(context, userManager.Object);

            var expectedResult = context.Users.First();
            var actualResult = await userService.GetByUsernameAsync(username);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetByUsernameAsync_WithNonExistingUserShouldReturnNull()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            var username = "user3";
            var userStore = new Mock<IUserStore<BookStoreUser>>();
            var userManager = new Mock<UserManager<BookStoreUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.FindByNameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.userService = new UserService(context, userManager.Object);

            var actualResult = await userService.GetByUsernameAsync(username);

            Assert.Null(actualResult);
        }
    }
}
