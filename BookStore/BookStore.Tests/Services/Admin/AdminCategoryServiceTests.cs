using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Admin;
using BookStore.Services.Admin.Models.Categories;
using BookStore.Services.Mapping;
using BookStore.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Tests.Services.Admin
{
    public class AdminCategoryServiceTests
    {
        private IAdminCategoryService categoryService;
        public AdminCategoryServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        private List<Category> GetTestData()
        {
            return new List<Category>()
            {
                new Category
                {
                    Name = "История",
                },
                new Category
                {
                    Name = "Икономика",
                }
            };
        }

        private async Task SeedData(BookStoreDbContext context)
        {
            context.AddRange(GetTestData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllCategories_WithData_ShouldReturnAllCategories()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new AdminCategoryService(context);


            List<AdminCategoryListingServiceModel> expectedData = GetTestData()
                .OrderBy(c => c.Name)
                .To<AdminCategoryListingServiceModel>().ToList();

            List<AdminCategoryListingServiceModel> actualData = await this.categoryService.GetAllCategories().ToListAsync();

            Assert.Equal(expectedData.Count, actualData.Count);

            for (int i = 0; i < expectedData.Count; i++)
            {
                var expectedEntry = expectedData[i];
                var actualEntry = actualData[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, "Name is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllCategories_WithoutData_ShouldReturnEmptyList()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            this.categoryService = new AdminCategoryService(context);

            List<AdminCategoryListingServiceModel> actualData = await this.categoryService.GetAllCategories().ToListAsync();

            Assert.Empty(actualData);
        }

        [Fact]
        public async Task GetAllActiveCategories_WithData_ShouldReturnAllActiveCategories()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new AdminCategoryService(context);


            List<AdminCategoryListingServiceModel> expectedData = GetTestData()
                .Where(c => c.IsDeleted == false)
                .To<AdminCategoryListingServiceModel>().ToList();
            List<AdminCategoryListingServiceModel> actualData = await this.categoryService.GetAllActiveCategories().ToListAsync();

            Assert.Equal(expectedData.Count, actualData.Count);

            foreach (var actualCategory in actualData)
            {
                Assert.True(expectedData.Any(category => actualCategory.Name == category.Name
                && actualCategory.IsDeleted == category.IsDeleted),
                    "AdminCategoryService GetAllActiveCategories() does not work properly");
            }
        }


        [Fact]
        public async Task GetAllActiveCategories_WithoutData_ShouldReturnEmptyList()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            this.categoryService = new AdminCategoryService(context);

            List<AdminCategoryListingServiceModel> actualData = await this.categoryService.GetAllActiveCategories().ToListAsync();

            Assert.Empty(actualData);
        }

        [Fact]
        public async Task GetAllActiveCategories_WithDataOrderedByNameAscending_ShouldReturnCorrectResults()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new AdminCategoryService(context);


            List<AdminCategoryListingServiceModel> expectedData = GetTestData()
                .Where(c => c.IsDeleted == false)
                .OrderBy(c => c.Name)
                .To<AdminCategoryListingServiceModel>().ToList();

            List<AdminCategoryListingServiceModel> actualData = await this.categoryService.GetAllActiveCategories().ToListAsync();

            Assert.Equal(expectedData.Count, actualData.Count);


            for (int i = 0; i < expectedData.Count; i++)
            {
                var expectedEntry = expectedData[i];
                var actualEntry = actualData[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, "Name is not returned properly.");
            }
        }

        [Fact]
        public async Task GetByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new AdminCategoryService(context);

            AdminCategoryListingServiceModel expectedData = context.Categories.First().To<AdminCategoryListingServiceModel>();
            AdminCategoryListingServiceModel actualData = await this.categoryService.GetByIdAsync(expectedData.Id);

            Assert.True(expectedData.Id == actualData.Id, "Id is not returned properly.");
            Assert.True(expectedData.Name == actualData.Name, "Name is not returned properly.");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistentId_ShouldReturnNull()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new AdminCategoryService(context);

            AdminCategoryListingServiceModel actualData = await this.categoryService.GetByIdAsync(int.MinValue);

            Assert.True(actualData == null);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateCategory()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new AdminCategoryService(context);

            bool actualResult = await this.categoryService.CreateAsync("Изкуство");
            Assert.True(actualResult);
        }

        [Fact]
        public async Task EditAsync_ShouldPassSuccessfully()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new AdminCategoryService(context);

            AdminCategoryListingServiceModel expectedData = context.Categories.First().To<AdminCategoryListingServiceModel>();

            bool actualData = await this.categoryService.EditAsync(expectedData.Id, expectedData.Name);

            Assert.True(actualData);
        }

        [Fact]
        public async Task EditAsync_ShouldEditCategoryCorrectly()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new AdminCategoryService(context);

            AdminCategoryListingServiceModel expectedData = context.Categories.First().To<AdminCategoryListingServiceModel>();

            expectedData.Name = "edited";

            await this.categoryService.EditAsync(expectedData.Id, expectedData.Name);

            AdminCategoryListingServiceModel actualData = context.Categories.First().To<AdminCategoryListingServiceModel>();

            Assert.True(actualData.Name == expectedData.Name,"Name not edited properly.");
        }

        [Fact]
        public async Task HideAsync_WithCorrectData_ShouldPassSuccessfully()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new AdminCategoryService(context);

            int id = context.Categories.First().Id;

            bool actualResult = await this.categoryService.HideAsync(id);

            Assert.True(actualResult);
        }


        [Fact]
        public async Task HideAsync_WithCorrectData_ShouldHideCategorySuccessfully()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new AdminCategoryService(context);

            int id = context.Categories.First().Id;

            bool actualResult = await this.categoryService.HideAsync(id);

            int expectedCount = 1;
            int actualCount = context.Categories.Where(c => c.IsDeleted == true).Count();

            Assert.True(actualResult);
            Assert.True(expectedCount == actualCount);
        }

        [Fact]
        public async Task HideAsync_WithNonExistentCategoryId_ShouldReturnFalse()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new AdminCategoryService(context);

            bool actualResult = await this.categoryService.HideAsync(-1);
         
            int expectedCount = 2;
            int actualCount = context.Categories.Where(c => c.IsDeleted == false).Count();

            Assert.False(actualResult);
            Assert.True(expectedCount == actualCount);
        }

        [Fact]
        public async Task ShowAsync_WithCorrectData_ShouldPassSuccessfully()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new AdminCategoryService(context);

            int id = context.Categories.First().Id;
            var category = context.Categories.First();
            category.IsDeleted = true;
            context.Categories.Update(category);
            await context.SaveChangesAsync();

            bool actualResult = await this.categoryService.ShowAsync(id);

            Assert.True(actualResult);
        }


        [Fact]
        public async Task ShowAsync_WithCorrectData_ShouldShowCategorySuccessfully()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new AdminCategoryService(context);

            int id = context.Categories.First().Id;

            var category = context.Categories.First();
            category.IsDeleted = true;
            context.Categories.Update(category);
            await context.SaveChangesAsync();

            bool actualResult = await this.categoryService.ShowAsync(id);

            int expectedCount = 2;
            int actualCount = context.Categories.Where(c => c.IsDeleted == false).Count();


            Assert.True(actualResult);
            Assert.True(expectedCount == actualCount);
        }

        [Fact]
        public async Task ShowAsync_WithNonExistentCategoryId_ShouldReturnFalse()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new AdminCategoryService(context);

            var category = context.Categories.First();
            category.IsDeleted = true;
            context.Categories.Update(category);
            await context.SaveChangesAsync();

            bool actualResult = await this.categoryService.ShowAsync(-1);

            int expectedCount = 1;
            int actualCount = context.Categories.Where(c => c.IsDeleted == true).Count();

            Assert.False(actualResult);
            Assert.True(expectedCount == actualCount);
        }
    }
}
