using BookStore.Data;
using BookStore.Domain;
using BookStore.Domain.Enums;
using BookStore.Services;
using BookStore.Services.Mapping;
using BookStore.Services.Models.Orders;
using BookStore.Services.Models.ShoppingCart;
using BookStore.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Tests.Services
{
    public class OrderServiceTests
    {
        private IOrderService orderService;
        private IShoppingCartService shoppingCartService;
        public OrderServiceTests()
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

        private List<Order> GetTestOrders()
        {
            var user = new BookStoreUser
            {
                UserName = "user1",
                Email = "user1@gmail.com",
                ShoppingCart = new ShoppingCart()
            };
            return new List<Order>()
            {
                new Order
                {
                    User = user,
                    Status = OrderStatus.Completed,
                    CompletionDate = DateTime.UtcNow.AddDays(-10),
                    OrderBooks = new List<OrderBook>
                    {
                        new OrderBook
                        {
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
                            }
                        },
                        new OrderBook
                        {
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
                            }
                        }
                    }
                },
                new Order
                {
                    User = user,
                    Status = OrderStatus.Completed,
                    CompletionDate = DateTime.UtcNow.AddDays(-15),
                    OrderBooks = new List<OrderBook>
                    {
                        new OrderBook
                        {
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
                            }
                        },
                    }
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
            //context.AddRange(GetTestOrders());
            await context.SaveChangesAsync();
        }


        [Fact]
        public async Task CreateAsync_ShouldCreateOrderSuccessfully()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            var userService = new Mock<IUserService>();
            var username = "user1";
            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);
            this.orderService = new OrderService(context, userService.Object, shoppingCartService);

            var book = context.Books.First();

            await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            var actualResult = await orderService.CreateAsync(username);

            Assert.True(actualResult);
        }

        [Fact]
        public async Task GetUserOrdersWithData_ShouldRetunAllUserOrders()
        {

            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            context.AddRange(GetTestOrders());
            await context.SaveChangesAsync();

            var userService = new Mock<IUserService>();
            var username = "user1";
            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);
            this.orderService = new OrderService(context, userService.Object, shoppingCartService);

            //var book = context.Books.First();

            //await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            //var actualResult = await orderService.CreateAsync(username);

            List<OrderListingServiceModel> expectedData = GetTestOrders()
                .To<OrderListingServiceModel>().ToList();

            List<OrderListingServiceModel> actualData = await this.orderService.GetUserOrders(username).ToListAsync();

            Assert.Equal(expectedData.Count, actualData.Count);

            //Assert.True(actualResult);
        }

        [Fact]
        public async Task GetUserOrdersWithoutData_ShouldRetunEmptyList()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();

            var userService = new Mock<IUserService>();
            var username = "user1";
            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);
            this.orderService = new OrderService(context, userService.Object, shoppingCartService);

            List<OrderListingServiceModel> actualData = await this.orderService.GetUserOrders(username).ToListAsync();

            Assert.Empty(actualData);
        }

        [Fact]
        public async Task GetOrderBooksAsync_ShouldReturnOrderBooks()
        {
            var context = BookStoreDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);

            var userService = new Mock<IUserService>();
            var username = "user1";
            userService.Setup(u => u.GetByUsernameAsync(username))
                       .Returns(context.Users.FirstOrDefaultAsync(x => x.UserName == username));

            this.shoppingCartService = new ShoppingCartService(context, userService.Object);
            this.orderService = new OrderService(context, userService.Object, shoppingCartService);

            var book = context.Books.First();

            await shoppingCartService.AddBookToShoppingCartAsync(book.Id, username);

            await orderService.CreateAsync(username);

            var order = context.Orders.First();
            var user = context.Users.First(u => u.UserName == username);

            var expectedResult = context.OrderBooks.Where(ob => ob.OrderId == order.Id).ToList().Count;

            var result = await orderService.GetOrderBooksAsync(order.Id);
            List<OrderBookListingServiceModel> actualData = new List<OrderBookListingServiceModel>(result);

            var actualResult = actualData.Count();
            Assert.True(expectedResult == actualResult);
        }
    }
}
