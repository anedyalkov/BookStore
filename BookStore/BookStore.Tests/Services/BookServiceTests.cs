using BookStore.Data;
using BookStore.Domain;
using BookStore.Services;
using BookStore.Services.Admin;
using BookStore.Services.Mapping;
using BookStore.Services.Models.Books;
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
    public class BookServiceTests
    {
        private IBookService bookService;
        private IAdminBookService adminBookService;
        public BookServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        private List<Book> GetTestData()
        {
            return new List<Book>()
            {
                new Book
                {
                   Title = "Свобода или смърт",
                   Author = new Author
                   {
                       FirstName = "Иван",
                       LastName = "Вазов"
                   },
                   Publisher = new Publisher
                   {
                       Name = "Иванов"
                   },

                   Language = "български",
                   Description = "описание",
                   Image = "image",
                   Price = 6.99M,
                   CreatedOn = DateTime.UtcNow.AddDays(-10)
                },
                new Book
                {
                   Title = "Иван",
                   Author = new Author
                   {
                       FirstName = "Георги",
                       LastName = "Иванов"
                   },
                   Publisher = new Publisher
                   {
                       Name = "Свобода"
                   },

                   Language = "български",
                   Description = "описание",
                   Image = "image",
                   Price = 5.99M,
                   CreatedOn = DateTime.UtcNow.AddDays(-20)
                }
            };
        }

        private async Task SeedData(BookStoreDbContext context)
        {
            context.AddRange(GetTestData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllActiveBooks_WithData_ShouldReturnAllActiveBooks()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new BookService(context);


            List<BookListingServiceModel> expectedData = GetTestData()
                .Where(c => c.IsDeleted == false)
                .To<BookListingServiceModel>().ToList();

            List<BookListingServiceModel> actualData = await this.bookService.GetAllActiveBooks().ToListAsync();

            Assert.Equal(expectedData.Count, actualData.Count);


            for (int i = 0; i < expectedData.Count; i++)
            {
                var expectedEntry = expectedData[i];
                var actualEntry = actualData[i];

                Assert.True(expectedEntry.Title == actualEntry.Title, "Title is not returned properly.");
                Assert.True(expectedEntry.AuthorFullName == actualEntry.AuthorFullName, "AuthorFullName is not returned properly.");
                Assert.True(expectedEntry.Price == actualEntry.Price, "Price is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllActiveAuthors_WithoutData_ShouldReturnEmptyList()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            this.bookService = new BookService(context);

            List<BookListingServiceModel> actualData = await this.bookService.GetAllActiveBooks().ToListAsync();

            Assert.Empty(actualData);
        }
        [Fact]
        public async Task GetByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new BookService(context);

            BookDetailsServiceModel expectedData = context.Books.First().To<BookDetailsServiceModel>();
            BookDetailsServiceModel actualData = await this.bookService.GetByIdAsync(expectedData.Id);

            Assert.True(expectedData.Id == actualData.Id, "Id is not returned properly.");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistentId_ShouldReturnNull()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new BookService(context);

            BookDetailsServiceModel actualData = await this.bookService.GetByIdAsync(int.MinValue);

            Assert.True(actualData == null);
        }

        [Fact]
        public async Task GetBooksByCategory_ShouldReturnAllCategoryBooks()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new BookService(context);
            this.adminBookService = new AdminBookService(context);

            var category = new Category
            {
                Name = "Изкуство"
            };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var bookFromDb = context.Books.First();
            var categoryFromDb = context.Categories.First();


            await this.adminBookService.AddCategoryAsync(bookFromDb.Id, categoryFromDb.Id);


            var expectedData = categoryFromDb.CategoryBooks.ToList();

            List<BookListingServiceModel> actualData = await this.bookService.GetBooksByCategory(categoryFromDb.Id).ToListAsync();

            Assert.Equal(expectedData.Count, actualData.Count);

            for (int i = 0; i < expectedData.Count; i++)
            {
                var expectedEntry = expectedData[i];
                var actualEntry = actualData[i];

                Assert.True(expectedEntry.Book.Title == actualEntry.Title, "Title is not returned properly.");
                Assert.True(expectedEntry.Book.Price == actualEntry.Price, "Price is not returned properly.");
                Assert.True(expectedEntry.Book.Author.FullName == actualEntry.AuthorFullName, "AuthorFullName is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBooksFilter_ShouldReturnAllCategoryBooks()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new BookService(context);
            this.adminBookService = new AdminBookService(context);

            var category = new Category
            {
                Name = "Изкуство"
            };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var bookFromDb = context.Books.First();
            var categoryFromDb = context.Categories.First();


            await this.adminBookService.AddCategoryAsync(bookFromDb.Id, categoryFromDb.Id);


            var expectedData = categoryFromDb.CategoryBooks.ToList();

            List<BookListingServiceModel> actualData = await this.bookService.GetBooksFilter(categoryFromDb.Id).ToListAsync();

            Assert.Equal(expectedData.Count, actualData.Count);

            for (int i = 0; i < expectedData.Count; i++)
            {
                var expectedEntry = expectedData[i];
                var actualEntry = actualData[i];

                Assert.True(expectedEntry.Book.Title == actualEntry.Title, "Title is not returned properly.");
                Assert.True(expectedEntry.Book.Price == actualEntry.Price, "Price is not returned properly.");
                Assert.True(expectedEntry.Book.Author.FullName == actualEntry.AuthorFullName, "AuthorFullName is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBooksFilter_ShouldReturnAllActiveBooks()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new BookService(context);
            this.adminBookService = new AdminBookService(context);

            var expectedData = bookService.GetAllActiveBooks().ToList();

            List<BookListingServiceModel> actualData = await this.bookService.GetBooksFilter(null).ToListAsync();

            Assert.Equal(expectedData.Count, actualData.Count);

            for (int i = 0; i < expectedData.Count; i++)
            {
                var expectedEntry = expectedData[i];
                var actualEntry = actualData[i];

                Assert.True(expectedEntry.Title == actualEntry.Title, "Title is not returned properly.");
                Assert.True(expectedEntry.Price == actualEntry.Price, "Price is not returned properly.");
                Assert.True(expectedEntry.AuthorFullName == actualEntry.AuthorFullName, "AuthorFullName is not returned properly.");
            }
        }

        [Theory]
        [InlineData("од")]
        [InlineData("свобода")]
        [InlineData("да")]
        [InlineData("св")]
        [InlineData("иван")]
        [InlineData("иванов")]
        public async Task FindBooks_ShouldReturnAllBooksByPublisher(string searchtext)
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new BookService(context);

            int expectedCount = 2;
            List<BookListingServiceModel> actualData = await this.bookService.FindBooks(searchtext).ToListAsync();

            Assert.Equal(expectedCount, actualData.Count);
        }

        [Fact]
        public async Task FindBooksByAuthorWithData_ShouldReturnAllAuthorBooks()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new BookService(context);

            var book = context.Books.First();
            var author = book.Author.FullName;

            int expectedCount = 1;
            List<BookListingServiceModel> actualData = await this.bookService.FindBooksByAuthor(author).ToListAsync();

            Assert.Equal(expectedCount, actualData.Count);
        }

        [Fact]
        public async Task FindBooksByAuthorWithoutData_ShouldReturnEmptyList()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            this.bookService = new BookService(context);

            List<BookListingServiceModel> actualData = await this.bookService.FindBooksByAuthor("Иван Вазов").ToListAsync();

            Assert.Empty(actualData);
        }

        [Fact]
        public async Task FindBooksByAuthor_ShouldReturnAllPublisherBooks()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new BookService(context);

            var book = context.Books.First();
            var publisher = book.Publisher.Name;

            int expectedCount = 1;
            List<BookListingServiceModel> actualData = await this.bookService.FindBooksByPublisher(publisher).ToListAsync();

            Assert.Equal(expectedCount, actualData.Count);
        }

        [Fact]
        public async Task FindBooksByPublisherWithoutData_ShouldReturnEmptyList()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            this.bookService = new BookService(context);

            List<BookListingServiceModel> actualData = await this.bookService.FindBooksByPublisher("Свобода").ToListAsync();

            Assert.Empty(actualData);
        }
    }
}


