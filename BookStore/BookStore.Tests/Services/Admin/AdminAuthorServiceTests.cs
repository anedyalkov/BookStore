using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Admin;
using BookStore.Services.Admin.Models.Authors;
using BookStore.Services.Mapping;
using BookStore.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Tests.Services.Admin
{
    public class AdminAuthorServiceTests
    {
        private IAdminAuthorService authorService;

        public AdminAuthorServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        private List<Author> GetTestData()
        {
            return new List<Author>()
            {
                new Author
                {
                    FirstName = "Иван",
                    LastName = "Вазов",
                    IsDeleted = false
                },
                new Author
                {
                    FirstName = "Стивън",
                    LastName = "Кинг",
                    IsDeleted = false
                }
            };
        }

        private async Task SeedData(BookStoreDbContext context)
        {
            context.AddRange(GetTestData());
            await context.SaveChangesAsync();
        }


        [Fact]
        public async Task GetAllAuthors_WithData_ShouldReturnAllAuthors()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.authorService = new AdminAuthorService(context);


            List<AdminAuthorListingServiceModel> expectedData = GetTestData()
                .To<AdminAuthorListingServiceModel>().ToList();

            List<AdminAuthorListingServiceModel> actualData = await this.authorService.GetAllAuthors().ToListAsync();

            Assert.Equal(expectedData.Count, actualData.Count);

            for (int i = 0; i < expectedData.Count; i++)
            {
                var expectedEntry = expectedData[i];
                var actualEntry = actualData[i];

                Assert.True(expectedEntry.FirstName == actualEntry.FirstName, "FirstName is not returned properly.");
                Assert.True(expectedEntry.LastName == actualEntry.LastName, "LastName is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllAuthors_WithoutData_ShouldReturnEmptyList()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            this.authorService = new AdminAuthorService(context);

            List<AdminAuthorListingServiceModel> actualData = await this.authorService.GetAllAuthors().ToListAsync();

            Assert.Empty(actualData);
        }

        [Fact]
        public async Task GetAllActiveAuthors_WithData_ShouldReturnAllActiveAuthors()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.authorService = new AdminAuthorService(context);


            List<AdminAuthorListingServiceModel> expectedData = GetTestData()
                .Where(c => c.IsDeleted == false)
                .To<AdminAuthorListingServiceModel>().ToList();

            List<AdminAuthorListingServiceModel> actualData = await this.authorService.GetAllActiveAuthors().ToListAsync();

            Assert.Equal(expectedData.Count, actualData.Count);


            for (int i = 0; i < expectedData.Count; i++)
            {
                var expectedEntry = expectedData[i];
                var actualEntry = actualData[i];

                Assert.True(expectedEntry.FirstName == actualEntry.FirstName, "FirstName is not returned properly.");
                Assert.True(expectedEntry.LastName == actualEntry.LastName, "LastName is not returned properly.");
                Assert.True(expectedEntry.IsDeleted == actualEntry.IsDeleted, "IsDeleted is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllActiveAuthors_WithoutData_ShouldReturnEmptyList()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            this.authorService = new AdminAuthorService(context);

            List<AdminAuthorListingServiceModel> actualData = await this.authorService.GetAllActiveAuthors().ToListAsync();

            Assert.Empty(actualData);
        }

        [Fact]
        public async Task GetByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.authorService = new AdminAuthorService(context);

            AdminAuthorListingServiceModel expectedData = context.Authors.First().To<AdminAuthorListingServiceModel>();
            AdminAuthorListingServiceModel actualData = await this.authorService.GetByIdAsync(expectedData.Id);

            Assert.True(expectedData.Id == actualData.Id, "Id is not returned properly.");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistentId_ShouldReturnNull()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.authorService = new AdminAuthorService(context);

            AdminAuthorListingServiceModel actualData = await this.authorService.GetByIdAsync(int.MinValue);

            Assert.True(actualData == null);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateAuthor()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.authorService = new AdminAuthorService(context);

            bool actualResult = await this.authorService.CreateAsync("Николай", "Хайтов");
            Assert.True(actualResult);
        }

        [Fact]
        public async Task EditAsync_ShouldEditCategory()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.authorService = new AdminAuthorService(context);

            AdminAuthorListingServiceModel expectedData = context.Authors.First().To<AdminAuthorListingServiceModel>();

            expectedData.FirstName = "edited";

            await this.authorService.EditAsync(expectedData.Id, expectedData.FirstName, expectedData.LastName);

            AdminAuthorListingServiceModel actualData = context.Authors.First().To<AdminAuthorListingServiceModel>();

            Assert.True(actualData.FirstName == expectedData.FirstName, "FirstName not edited properly.");
        }

        [Fact]
        public async Task HideAsync_ShouldHideAuthor()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.authorService = new AdminAuthorService(context);

            int id = context.Authors.First().To<AdminAuthorListingServiceModel>().Id;

            bool actualResult = await this.authorService.HideAsync(id);

            int expectedCount = 1;
            int actualCount = context.Authors.Where(c => c.IsDeleted == true).Count();


            Assert.True(actualResult);
            Assert.True(expectedCount == actualCount);
        }

        [Fact]
        public async Task HideAsync_WithNonExistentAuthorId_ShouldReturnFalse()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.authorService = new AdminAuthorService(context);

            bool actualResult = await this.authorService.HideAsync(int.MinValue);

            int expectedCount = 2;
            int actualCount = context.Authors.Count();

            Assert.False(actualResult);
            Assert.True(expectedCount == actualCount);
        }

        [Fact]
        public async Task ShowAsync_WithCorrectData_ShouldShowAuthor()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.authorService = new AdminAuthorService(context);

            int id = context.Authors.First().Id;

            var author = context.Authors.First();
            author.IsDeleted = true;
            context.Authors.Update(author);
            await context.SaveChangesAsync();

            bool actualResult = await this.authorService.ShowAsync(id);

            int expectedCount = 2;
            int actualCount = context.Authors.Where(a => a.IsDeleted == false).Count();

            Assert.True(actualResult);
            Assert.True(expectedCount == actualCount);
        }

        [Fact]
        public async Task ShowAsync_WithNonExistentAuthorId_ShouldReturnFalse()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.authorService = new AdminAuthorService(context);

            var author = context.Authors.First();
            author.IsDeleted = true;
            context.Authors.Update(author);
            await context.SaveChangesAsync();

            bool actualResult = await this.authorService.ShowAsync(-1);

            int expectedCount = 1;
            int actualCount = context.Authors.Where(c => c.IsDeleted == true).Count();

            Assert.False(actualResult);
            Assert.True(expectedCount == actualCount);
        }
    }
}
