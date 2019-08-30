using BookStore.Data;
using BookStore.Domain;
using BookStore.Services;
using BookStore.Services.Models.ShoppingCart;
using BookStore.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Tests.Services
{
    public class ShoppingCartServiceTests
    {
        private IShoppingCartService shoppingCartService;

        public ShoppingCartServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        private List<BookStoreUser> GetTestUsers()
        {
            return new List<BookStoreUser>()
            {
                new BookStoreUser
                {
                    UserName = "user1",
                    Email = "user1@gmail.com",
                    ShoppingCart = new ShoppingCart()
                },
                new BookStoreUser
                {
                   UserName = "user2",
                   Email = "user2@gmail.com",
                   ShoppingCart = new ShoppingCart()
                }
            };
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
            context.AddRange(GetTestUsers());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task AddBookToShoppingCartAsync_ShouldAddBookToCart()
        {     
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            var userService = new Mock<IUserService>();
            var username = "user1";
            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);

            var book = context.Books.First();

            var actualResult = await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            Assert.True(actualResult);
        }

        [Fact]
        public async Task AddBookToShoppingCartAsync_WithNonExistingUser_ShouldNotAddBookToCart()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            var username = "user3";
            var userService = new Mock<IUserService>();
            
            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);

            var book = context.Books.First();

            var actualResult = await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            var shoppingCartBooks = context.ShoppingCartBooks.ToList();

            Assert.Empty(shoppingCartBooks);
            Assert.False(actualResult);
        }

        [Fact]
        public async Task AddBookToShoppingCartAsync_WithNonExistingBook_ShouldNotAddBookToCart()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            var username = "user1";
            var userService = new Mock<IUserService>();

            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);

            var actualResult = await shoppingCartService.AddBookToShoppingCartAsync(-1, username);

            var shoppingCartBooks = context.ShoppingCartBooks.ToList();

            Assert.Empty(shoppingCartBooks);
            Assert.False(actualResult);
        }

        [Fact]
        public async Task AddBookToShoppingCartAsync_WithExistingBookInCart_ShouldNotAddBookToCart()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            var username = "user1";
            var userService = new Mock<IUserService>();

            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);

            var book = context.Books.First();

            await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            var actualResult = await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            var shoppingCartBooks = context.ShoppingCartBooks.ToList();

            Assert.Single(shoppingCartBooks);
            Assert.False(actualResult);
        }

        [Fact]
        public async Task RemoveBookFromShoppingCartAsync_ShouldRemoveBook()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            var userService = new Mock<IUserService>();
            var username = "user1";
            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);

            var book = context.Books.First();

            await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            var actualResult = await shoppingCartService.RemoveBookFromShoppingCartAsync(book.Id, username);

            Assert.True(actualResult);
        }

        [Fact]
        public async Task GetUserCartBooksAsync_ShouldReturnUserCartBooks()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            var userService = new Mock<IUserService>();
            var username = "user1";
            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);

            var book = context.Books.First();
            var user = context.Users.First(u => u.UserName == username);

            await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            var expectedResult = context.ShoppingCartBooks.Where(shb => shb.ShoppingCartId == user.ShoppingCartId).ToList().Count;

            var result = await shoppingCartService.GetUserCartBooksAsync(username);
            List<ShoppingCartServiceModel> actualData = new List<ShoppingCartServiceModel>(result);
            var actualResult = actualData.Count();
            Assert.True(expectedResult == actualResult);
        }

        [Fact]
        public async Task IncreaseQuantityAsync_ShouldIncreaseBookQuantity()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            var userService = new Mock<IUserService>();
            var username = "user1";
            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);

            var book = context.Books.First();

            await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            var actualResult = await shoppingCartService.IncreaseQuantityAsync(book.Id, username);

            var expectedQuantity = 2;
            var shoppingCartBook = context.ShoppingCartBooks.First();

            var actualQuantity = shoppingCartBook.Quantity;

            Assert.True(actualResult);
            Assert.Equal(expectedQuantity, actualQuantity);
        }

        [Fact]
        public async Task IncreaseQuantityAsync_WithNonExistingUser_ShouldNotIncreaseBookQuantity()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            var userService = new Mock<IUserService>();
            var username = "user1";
            var username1 = "user3";
            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);

            var book = context.Books.First();

            await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            var actualResult = await shoppingCartService.IncreaseQuantityAsync(book.Id, username1);

            var expectedQuantity = 1;
            var shoppingCartBook = context.ShoppingCartBooks.First();

            var actualQuantity = shoppingCartBook.Quantity;

            Assert.False(actualResult);
            Assert.Equal(expectedQuantity, actualQuantity);
        }

        [Fact]
        public async Task IncreaseQuantityAsync_WithNonExistingBook_ShouldNotIncreaseBookQuantity()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            var userService = new Mock<IUserService>();
            var username = "user1";
            var username1 = "user3";
            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);

            var book = context.Books.First();

            await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            var actualResult = await shoppingCartService.IncreaseQuantityAsync(-1, username1);

            var expectedQuantity = 1;
            var shoppingCartBook = context.ShoppingCartBooks.First();

            var actualQuantity = shoppingCartBook.Quantity;

            Assert.False(actualResult);
            Assert.Equal(expectedQuantity, actualQuantity);
        }

        [Fact]
        public async Task DecreaseQuantityAsync_ShouldDecreaseBookQuantity()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            var userService = new Mock<IUserService>();
            var username = "user1";
            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);

            var book = context.Books.First();

            await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            await shoppingCartService.IncreaseQuantityAsync(book.Id, username);
            var actualResult = await shoppingCartService.DecreaseQuantityAsync(book.Id, username);

            var expectedQuantity = 1;
            var shoppingCartBook = context.ShoppingCartBooks.First();

            var actualQuantity = shoppingCartBook.Quantity;

            Assert.True(actualResult);
            Assert.Equal(expectedQuantity, actualQuantity);
        }

        [Fact]
        public async Task DecreaseQuantityAsync_WithtQuantityOne_ShouldNotDecreaseBookQuantity()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            var userService = new Mock<IUserService>();
            var username = "user1";
            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);

            var book = context.Books.First();

            await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            var actualResult = await shoppingCartService.DecreaseQuantityAsync(book.Id, username);

            var expectedQuantity = 1;
            var shoppingCartBook = context.ShoppingCartBooks.First();

            var actualQuantity = shoppingCartBook.Quantity;

            Assert.False(actualResult);
            Assert.Equal(expectedQuantity, actualQuantity);
        }

        [Fact]
        public async Task DecreaseQuantityAsync_WithNonExistingUser_ShouldNotDecreaseBookQuantity()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            var userService = new Mock<IUserService>();
            var username = "user1";
            var username1 = "user3";
            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);

            var book = context.Books.First();

            await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            await shoppingCartService.IncreaseQuantityAsync(book.Id, username);

            var actualResult = await shoppingCartService.DecreaseQuantityAsync(book.Id, username1);

            var expectedQuantity = 2;
            var shoppingCartBook = context.ShoppingCartBooks.First();

            var actualQuantity = shoppingCartBook.Quantity;

            Assert.False(actualResult);
            Assert.Equal(expectedQuantity, actualQuantity);
        }

        [Fact]
        public async Task DecreaseQuantityAsync_WithNonExistingBook_ShouldNotDecreaseBookQuantity()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            var userService = new Mock<IUserService>();
            var username = "user1";
            var username1 = "user3";
            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);

            var book = context.Books.First();

            await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            await shoppingCartService.IncreaseQuantityAsync(book.Id, username);

            var actualResult = await shoppingCartService.DecreaseQuantityAsync(-1, username1);

            var expectedQuantity = 2;
            var shoppingCartBook = context.ShoppingCartBooks.First();

            var actualQuantity = shoppingCartBook.Quantity;

            Assert.False(actualResult);
            Assert.Equal(expectedQuantity, actualQuantity);
        }

        [Fact]
        public async Task RemoveBooksFromShoppingCartAsync_ShouldRemoveBooks()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            var userService = new Mock<IUserService>();
            var username = "user1";
            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);

            var book = context.Books.First();

            await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            var actualResult = await shoppingCartService.RemoveBooksFromShoppingCartAsync(username);
            var shoppingCartBooks = context.ShoppingCartBooks.ToList();

            Assert.True(actualResult);
            Assert.Empty(shoppingCartBooks);
        }
    }
}

