using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Admin;
using BookStore.Services.Admin.Models.Books;
using BookStore.Services.Mapping;
using BookStore.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Tests.Services.Admin
{
    public class AdminBookServiceTests
    {
        private IAdminBookService bookService;

        public AdminBookServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        private List<Book> GetTestData()
        {
            return new List<Book>()
            {
                new Book
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
                new Book
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
                }
            };
        }

        private async Task SeedData(BookStoreDbContext context)
        {
            context.AddRange(GetTestData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateBook()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new AdminBookService(context);

            bool actualResult = await this.bookService.CreateAsync(
                "Стихотворения",
                1,
                1,
                "български",
                "описание",
                "снимка",
                 DateTime.UtcNow,
                 12);
            Assert.True(actualResult);
        }

        [Fact]
        public async Task GetAllBooks_WithData_ShouldReturnAllBooks()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new AdminBookService(context);


            List<AdminBookListingServiceModel> expectedData = GetTestData()
                .To<AdminBookListingServiceModel>().ToList();

            List<AdminBookListingServiceModel> actualData = await this.bookService.GetAllBooks().ToListAsync();

            Assert.Equal(expectedData.Count, actualData.Count);


            for (int i = 0; i < expectedData.Count; i++)
            {
                var expectedEntry = expectedData[i];
                var actualEntry = actualData[i];

                Assert.True(expectedEntry.Title == actualEntry.Title, "Title is not returned properly.");
                Assert.True(expectedEntry.AuthorFullName == actualEntry.AuthorFullName, "AuthorFullName is not returned properly.");
                Assert.True(expectedEntry.PublisherName == actualEntry.PublisherName, "PublisherName is not returned properly.");
                Assert.True(expectedEntry.Price == actualEntry.Price, "Price is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBooksByPublisherId_ShouldReturnAllPublisherBooks()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new AdminBookService(context);

            var book = context.Books.First();
            var publisher = book.Publisher;

            int expectedCount = 1;
            List<AdminBookListingServiceModel> actualData = await this.bookService.GetBooksByPublisherId(publisher.Id).ToListAsync();

            Assert.Equal(expectedCount, actualData.Count);
        }

        [Fact]
        public async Task GetBooksByCategory_ShouldReturnAllCategoryBooks()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            //this.bookService = new BookService(context);
            this.bookService = new AdminBookService(context);

            var category = new Category
            {
                Name = "Изкуство"
            };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var bookFromDb = context.Books.First();
            var categoryFromDb = context.Categories.First();

            await this.bookService.AddCategoryAsync(bookFromDb.Id, categoryFromDb.Id);

            var expectedData = categoryFromDb.CategoryBooks.ToList();

            List<AdminBookListingServiceModel> actualData = await this.bookService.GetBooksByCategoryId(categoryFromDb.Id).ToListAsync();

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
        public async Task GetBooksByAuthorId_ShouldReturnAllAuthorBooks()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new AdminBookService(context);

            var book = context.Books.First();
            var author = book.Author;

            int expectedCount = 1;
            List<AdminBookListingServiceModel> actualData = await this.bookService.GetBooksByAuthorId(author.Id).ToListAsync();

            Assert.Equal(expectedCount, actualData.Count);
        }

        [Fact]
        public async Task GetAllBooks_WithoutData_ShouldReturnEmptyList()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            this.bookService = new AdminBookService(context);

            List<AdminBookListingServiceModel> actualData = await this.bookService.GetAllBooks().ToListAsync();

            Assert.Empty(actualData);
        }

        [Fact]
        public async Task GetByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new AdminBookService(context);

            AdminBookListingServiceModel expectedData = context.Books.First().To<AdminBookListingServiceModel>();
            AdminBookListingServiceModel actualData = await this.bookService.GetByIdAsync<AdminBookListingServiceModel>(expectedData.Id);

            Assert.True(expectedData.Id == actualData.Id, "Id is not returned properly.");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistentId_ShouldReturnNull()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new AdminBookService(context);

            AdminBookListingServiceModel actualData = await this.bookService.GetByIdAsync<AdminBookListingServiceModel>(int.MinValue);

            Assert.True(actualData == null);
        }

        [Fact]
        public async Task AddCategoryAsync_ShouldAddCategoryToBook()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
           
            var category = new Category
            {
                Name = "Изкуство"
            };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var bookFromDb = context.Books.First();
            var categoryFromDb = context.Categories.First();

            this.bookService = new AdminBookService(context);

            bool actualResult = await this.bookService.AddCategoryAsync(bookFromDb.Id, categoryFromDb.Id);
            Assert.True(actualResult);
        }

        [Fact]
        public async Task AddCategoryAsync_WithNonExistingCategory_ShouldRetunFalse()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            this.bookService = new AdminBookService(context);

            bool actualResult = await this.bookService.AddCategoryAsync(1, 1);
            Assert.False(actualResult);
        }

        [Fact]
        public async Task AddCategoryAsync_WithNonExistingBookIdShouldRetunFalse()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            var category = new Category
            {
                Name = "Художествена литература"
            };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            this.bookService = new AdminBookService(context);

            bool actualResult = await this.bookService.AddCategoryAsync(-1, 1);
            Assert.False(actualResult);
        }

        [Fact]
        public async Task RemoveCategoryAsync_ShouldRemoveCategoryFromBook()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            var category = new Category
            {
                Name = "История"
            };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var bookFromDb = context.Books.First();
            var categoryFromDb = context.Categories.First();

            this.bookService = new AdminBookService(context);

            await this.bookService.AddCategoryAsync(bookFromDb.Id, categoryFromDb.Id);

            bool actualResult = await this.bookService.RemoveCategoryAsync(bookFromDb.Id, categoryFromDb.Id);
            Assert.True(actualResult);
        }

        [Fact]
        public async Task RemoveCategoryAsync_WithNonExistingCategoryIdShouldReturnFalse()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            var category = new Category
            {
                Name = "Художествена литература"
            };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var bookFromDb = context.Books.First();
            var categoryFromDb = context.Categories.First();

            this.bookService = new AdminBookService(context);

            await this.bookService.AddCategoryAsync(bookFromDb.Id, categoryFromDb.Id);

            bool actualResult = await this.bookService.RemoveCategoryAsync(bookFromDb.Id, -1);
            Assert.False(actualResult);
        }

        [Fact]
        public async Task RemoveCategoryAsync_WithNonExistingBookIdShouldReturnFalse()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            var category = new Category
            {
                Name = "Художествена литература"
            };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var bookFromDb = context.Books.First();
            var categoryFromDb = context.Categories.First();

            this.bookService = new AdminBookService(context);

            await this.bookService.AddCategoryAsync(bookFromDb.Id, categoryFromDb.Id);

            bool actualResult = await this.bookService.RemoveCategoryAsync(-1, categoryFromDb.Id);
            Assert.False(actualResult);
        }

        [Fact]
        public async Task EditAsync_ShouldEditBook()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new AdminBookService(context);

            var expectedData = context.Books.First();

            expectedData.Title = "Под";
            expectedData.AuthorId = 2;
            expectedData.PublisherId = 1;
            expectedData.Language = "бълг";
            expectedData.Description = "опис";
            expectedData.Image = "editedImage";
            expectedData.Price = 4.99M;

            await this.bookService.EditAsync(expectedData.Id,
                expectedData.Title, expectedData.AuthorId, expectedData.PublisherId,
                expectedData.Language, expectedData.Description, expectedData.Image, expectedData.CreatedOn,
                expectedData.Price);

            var actualData = context.Books.First();

            Assert.True(actualData.Title == expectedData.Title, "Title not edited properly.");
            Assert.True(actualData.AuthorId == expectedData.AuthorId, "Author not edited properly.");
            Assert.True(actualData.PublisherId == expectedData.PublisherId, "Publisher not edited properly.");
            Assert.True(actualData.Language == expectedData.Language, "Language not edited properly.");
            Assert.True(actualData.Description == expectedData.Description, "Description not edited properly.");
            Assert.True(actualData.Image == expectedData.Image, "Image not edited properly.");
            Assert.True(actualData.Price == expectedData.Price, "Price not edited properly.");
        }

        [Fact]
        public async Task HideAsync_ShouldHideBook()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new AdminBookService(context);

            int id = context.Books.First().To<AdminBookListingServiceModel>().Id;

            bool actualResult = await this.bookService.HideAsync(id);

            int expectedCount = 1;
            int actualCount = context.Books.Where(c => c.IsDeleted == true).Count();


            Assert.True(actualResult);
            Assert.True(expectedCount == actualCount);
        }

        [Fact]
        public async Task HideAsync_WithNonExistentBookId_ShouldReturnFalse()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new AdminBookService(context);

            bool actualResult = await this.bookService.HideAsync(-1);

            int expectedCount = 2;
            int actualCount = context.Books.Count();

            Assert.False(actualResult);
            Assert.True(expectedCount == actualCount);
        }

        [Fact]
        public async Task ShowAsync_ShouldShowBook()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new AdminBookService(context);

            int id = context.Books.First().Id;
            var book = context.Books.First();
            book.IsDeleted = true;
            context.Books.Update(book);
            await context.SaveChangesAsync();

            bool actualResult = await this.bookService.ShowAsync(id);

            int expectedCount = 2;
            int actualCount = context.Books.Where(a => a.IsDeleted == false).Count();

            Assert.True(actualResult);
            Assert.True(expectedCount == actualCount);
        }

        [Fact]
        public async Task ShowAsync_WithNonExistentBookId_ShouldReturnFalse()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.bookService = new AdminBookService(context);

            int id = context.Books.First().Id;
            var book = context.Books.First();
            book.IsDeleted = true;
            context.Books.Update(book);
            await context.SaveChangesAsync();

            bool actualResult = await this.bookService.ShowAsync(-1);

            int expectedCount = 1;
            int actualCount = context.Books.Where(c => c.IsDeleted == true).Count();

            Assert.False(actualResult);
            Assert.True(expectedCount == actualCount);
        }
    }
}
