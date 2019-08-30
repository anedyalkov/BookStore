using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Admin;
using BookStore.Services.Admin.Models.Publishers;
using BookStore.Services.Mapping;
using BookStore.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Tests.Services.Admin
{
    public class AdminPublisherServiceTests
    {
        private IAdminPublisherService publisherService;

        public AdminPublisherServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        private List<Publisher> GetTestData()
        {
            return new List<Publisher>()
            {
                new Publisher
                {
                    Name = "Свобода",
                    IsDeleted = false
                },
                new Publisher
                {
                    Name = "Култура",
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
        public async Task GetAllPublishers_WithData_ShouldReturnAllPublishers()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.publisherService = new AdminPublisherService(context);


            List<AdminPublisherListingServiceModel> expectedData = GetTestData()
                .To<AdminPublisherListingServiceModel>().ToList();

            List<AdminPublisherListingServiceModel> actualData = await this.publisherService.GetAllPublishers().ToListAsync();
             
            Assert.Equal(expectedData.Count, actualData.Count);

            for (int i = 0; i < expectedData.Count; i++)
            {
                var expectedEntry = expectedData[i];
                var actualEntry = actualData[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, "Name is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllPublishers_WithoutData_ShouldReturnEmptyList()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            this.publisherService = new AdminPublisherService(context);

            List<AdminPublisherListingServiceModel> actualData = await this.publisherService.GetAllPublishers().ToListAsync();

            Assert.Empty(actualData);
        }


        [Fact]
        public async Task GetAllActivePublishers_WithData_ShouldReturnAllActivePublishers()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.publisherService = new AdminPublisherService(context);


            List<AdminPublisherListingServiceModel> expectedData = GetTestData()
                .Where(c => c.IsDeleted == false)
                .To<AdminPublisherListingServiceModel>().ToList();

            List<AdminPublisherListingServiceModel> actualData = await this.publisherService.GetAllActivePublishers().ToListAsync();

            Assert.Equal(expectedData.Count, actualData.Count);

            for (int i = 0; i < expectedData.Count; i++)
            {
                var expectedEntry = expectedData[i];
                var actualEntry = actualData[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, "Name is not returned properly.");
            }
        }


        [Fact]
        public async Task GetAllActivePublishers_WithoutData_ShouldReturnEmptyList()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            this.publisherService = new AdminPublisherService(context);

            List<AdminPublisherListingServiceModel> actualData = await this.publisherService.GetAllActivePublishers().ToListAsync();

            Assert.Empty(actualData);
        }


        [Fact]
        public async Task GetByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.publisherService = new AdminPublisherService(context);

            AdminPublisherListingServiceModel expectedData = context.Publishers.First().To<AdminPublisherListingServiceModel>();
            AdminPublisherListingServiceModel actualData = await this.publisherService.GetByIdAsync(expectedData.Id);

            Assert.True(expectedData.Id == actualData.Id, "Id is not returned properly.");
            Assert.True(expectedData.Name == actualData.Name, "Name is not returned properly.");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistentId_ShouldReturnNull()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.publisherService = new AdminPublisherService(context);

            AdminPublisherListingServiceModel actualData = await this.publisherService.GetByIdAsync(int.MinValue);

            Assert.True(actualData == null);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateCategory()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.publisherService = new AdminPublisherService(context);

            bool actualResult = await this.publisherService.CreateAsync("Изкуство");
            Assert.True(actualResult);
        }

        [Fact]
        public async Task EditAsync_ShouldEditCategory()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.publisherService = new AdminPublisherService(context);

            AdminPublisherListingServiceModel expectedData = context.Publishers.First().To<AdminPublisherListingServiceModel>();

            expectedData.Name = "edited";

            await this.publisherService.EditAsync(expectedData.Id, expectedData.Name);

            AdminPublisherListingServiceModel actualData = context.Publishers.First().To<AdminPublisherListingServiceModel>();

            Assert.True(actualData.Name == expectedData.Name, "Name not edited properly.");
        }

        [Fact]
        public async Task HideAsync_ShouldHidePublisher()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.publisherService = new AdminPublisherService(context);

            int id = context.Publishers.First().To<AdminPublisherListingServiceModel>().Id;

            bool actualResult = await this.publisherService.HideAsync(id);

            int expectedCount = 1;
            int actualCount = context.Publishers.Where(c => c.IsDeleted == true).Count();


            Assert.True(actualResult);
            Assert.True(expectedCount == actualCount);
        }

        [Fact]
        public async Task HideAsync_WithNonExistentPublisherId_ShouldReturnFalse()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.publisherService = new AdminPublisherService(context);

            bool actualResult = await this.publisherService.HideAsync(-1);


            int expectedCount = 2;
            int actualCount = context.Publishers.Count();

            Assert.False(actualResult);
            Assert.True(expectedCount == actualCount);
        }

        [Fact]
        public async Task ShowAsync_ShouldShowCategoryy()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.publisherService = new AdminPublisherService(context);

            int id = context.Publishers.First().Id;

            var publisher = context.Publishers.First();
            publisher.IsDeleted = true;
            context.Publishers.Update(publisher);
            await context.SaveChangesAsync();

            bool actualResult = await this.publisherService.ShowAsync(id);

            int expectedCount = 2;
            int actualCount = context.Publishers.Count();

            Assert.True(actualResult);
            Assert.True(expectedCount == actualCount);
        }

        [Fact]
        public async Task ShowAsync_WithNonExistentPublisherId_ShouldReturnFalse()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.publisherService = new AdminPublisherService(context);

            var publisher = context.Publishers.First();
            publisher.IsDeleted = true;
            context.Publishers.Update(publisher);
            await context.SaveChangesAsync();

            bool actualResult = await this.publisherService.ShowAsync(-1);

            int expectedCount = 1;
            int actualCount = context.Publishers.Where(c => c.IsDeleted == true).Count();

            Assert.False(actualResult);
            Assert.True(expectedCount == actualCount);
        }
    }
}
