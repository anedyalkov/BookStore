using BookStore.Data;
using BookStore.Domain;
using BookStore.Services;
using BookStore.Services.Mapping;
using BookStore.Services.Models.Books;
using BookStore.Services.Models.Reviews;
using BookStore.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Tests.Services
{
    public class ReviewServiceTests
    {
        IReviewService reviewService;
        public ReviewServiceTests()
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
        public async Task CreateAsync_ShouldSuccessfullyCreateBook()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.reviewService = new ReviewService(context);

            var book = context.Books.First();
            var creator = context.Users.First();
            bool actualResult = await this.reviewService.CreateAsync(book.Id, "cool", creator.Id);
            Assert.True(actualResult);
        }

        [Fact]
        public async Task GetReviewsByBook_ShouldReturnAllReviews()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.reviewService = new ReviewService(context);

            var book = context.Books.First().To<BookListingServiceModel>();

            var expectedData = book.Reviews
                .OrderByDescending(r => r.CreatedOn)
                .ToList();

            List<ReviewListingServiceModel> actualData = await this.reviewService.GetReviewsByBook(book.Id).ToListAsync();

            Assert.Equal(expectedData.Count, actualData.Count);

            for (int i = 0; i < expectedData.Count; i++)
            {
                var expectedEntry = expectedData[i];
                var actualEntry = actualData[i];

                Assert.True(expectedEntry.CreatorUserName == actualEntry.CreatorUserName, "CreatorUserName is not returned properly.");
                Assert.True(expectedEntry.Text == actualEntry.Text, "Text is not returned properly.");
            }
        }
    }
}
