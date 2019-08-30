using BookStore.Data;
using BookStore.Domain;
using BookStore.Services;
using BookStore.Services.Mapping;
using BookStore.Services.Models.Categories;
using BookStore.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Tests.Services
{
    public class CategoryServiceTests
    {
        private ICategoryService categoryService;

        public CategoryServiceTests()
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
        public async Task GetAllActiveCategories_WithData_ShouldReturnAllActiveCategories()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new CategoryService(context);

            List<CategoryListingServiceModel> expectedData = GetTestData()
                .Where(c => c.IsDeleted == false)
                .To<CategoryListingServiceModel>().ToList();

            List<CategoryListingServiceModel> actualData = await this.categoryService.GetAllActiveCategories().ToListAsync();

            Assert.Equal(expectedData.Count, actualData.Count);

            for (int i = 0; i < expectedData.Count; i++)
            {
                var expectedEntry = expectedData[i];
                var actualEntry = actualData[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, "Name is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllActiveCategories_WithoutData_ShouldReturnEmptyList()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            this.categoryService = new CategoryService(context);

            List<CategoryListingServiceModel> actualData = await this.categoryService.GetAllActiveCategories().ToListAsync();

            Assert.Empty(actualData);
        }

        [Fact]
        public async Task GetByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new CategoryService(context);

            CategoryListingServiceModel expectedData = context.Categories.First().To<CategoryListingServiceModel>();
            CategoryListingServiceModel actualData = await this.categoryService.GetByIdAsync(expectedData.Id);

            Assert.True(expectedData.Id == actualData.Id, "Id is not returned properly.");
            Assert.True(expectedData.Name == actualData.Name, "Name is not returned properly.");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistentId_ShouldReturnNull()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.categoryService = new CategoryService(context);

            CategoryListingServiceModel actualData = await this.categoryService.GetByIdAsync(-1);

            Assert.True(actualData == null);
        }
    }
}
