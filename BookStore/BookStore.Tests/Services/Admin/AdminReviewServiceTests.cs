using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Admin;
using BookStore.Services.Admin.Models.Reviews;
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
    public class AdminReviewServiceTests
    {
        private IAdminReviewService reviewService;
        public AdminReviewServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        private List<Review> GetTestData()
        {
            return new List<Review>()
            {
                new Review
                {
                   Text = "text",
                   Creator = new BookStoreUser
                   {
                      UserName = "user1",
                      Email = "user1@gmail.com",
                   },
                   Book = new Book
                   {
                       Title = "Под Игото",
                       Author = new Author
                       {
                           FirstName = "Иван",
                           LastName = "Вазов"
                       },
                       Publisher = new Publisher
                       { 
                           Name = "Култура"
                       },

                       Language = "български",
                       Description = "описание",
                       Image = "image",
                       Price = 6.99M,
                       CreatedOn = DateTime.UtcNow.AddDays(-10)
                   },
                   CreatedOn = DateTime.UtcNow.AddDays(-10)
                },
                 new Review
                 {
                   Text = "text1",
                   Creator = new BookStoreUser
                   {
                      UserName = "user2",
                      Email = "user2@gmail.com",
                   },
                   Book = new Book
                   {
                       Title = "Науката на успеха",
                       Author = new Author
                       {
                          FirstName = "Наполеон",
                          LastName = "Хил"
                       },
                       Publisher = new Publisher
                       {
                          Name = "Култура"
                       },
                       Language = "български",
                       Description = "описание",
                       Image = "image",
                       Price = 5.99M,
                       CreatedOn = DateTime.UtcNow.AddDays(-20)
                   },
                   CreatedOn = DateTime.UtcNow.AddDays(-15)
                 }
            };
        }

        private async Task SeedData(BookStoreDbContext context)
        {
            context.AddRange(GetTestData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.reviewService = new AdminReviewService(context);

            AdminReviewListingServiceModel expectedData = context.Reviews.First().To<AdminReviewListingServiceModel>();
            AdminReviewListingServiceModel actualData = await this.reviewService.GetByIdAsync(expectedData.Id);

            Assert.True(expectedData.Id == actualData.Id, "Id is not returned properly.");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistentId_ShouldReturnNull()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.reviewService = new AdminReviewService(context);

            AdminReviewListingServiceModel actualData = await this.reviewService.GetByIdAsync(int.MinValue);

            Assert.True(actualData == null);
        }

        [Fact]
        public async Task GetAllReviews_WithData_ShouldReturnAllReviews()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.reviewService = new AdminReviewService(context);


            List<AdminReviewListingServiceModel> expectedData = GetTestData()
                .To<AdminReviewListingServiceModel>().ToList();

            List<AdminReviewListingServiceModel> actualData = await this.reviewService.GetAllReviews().ToListAsync();

            Assert.Equal(expectedData.Count, actualData.Count);


            for (int i = 0; i < expectedData.Count; i++)
            {
                var expectedEntry = expectedData[i];
                var actualEntry = actualData[i];

                Assert.True(expectedEntry.Creator == actualEntry.Creator, "Creator is not returned properly.");
                Assert.True(expectedEntry.Book == actualEntry.Book, "Book is not returned properly.");
                Assert.True(expectedEntry.Text == actualEntry.Text, "Text is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllReviews_WithoutData_ShouldReturnEmptyList()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            this.reviewService = new AdminReviewService(context);

            List<AdminReviewListingServiceModel> actualData = await this.reviewService.GetAllReviews().ToListAsync();

            Assert.Empty(actualData);
        }

        [Fact]
        public async Task RemoveAsync_WithCorrectData_ShouldDeleteReviewSuccessfully()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.reviewService = new AdminReviewService(context);

            int id = context.Reviews.First().Id;

            await this.reviewService.RemoveAsync(id);

            int expectedCount = 1;
            int actualCount = context.Reviews.Count();

            Assert.True(expectedCount == actualCount);
        }

        [Fact]
        public async Task RemoveAsync_WithNonExistingId_ShouldRetunFalse()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.reviewService = new AdminReviewService(context);

            var actualResult = await this.reviewService.RemoveAsync(-1);

            int expectedCount = 2;
            int actualCount = context.Reviews.Count();

            Assert.False(actualResult);
            Assert.True(expectedCount == actualCount);
        }
    }
}
