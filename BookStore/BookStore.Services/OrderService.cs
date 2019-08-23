using BookStore.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using BookStore.Domain.Enums;
using System.Threading.Tasks;
using BookStore.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BookStore.Services.Models.Orders;
using BookStore.Services.Mapping;

namespace BookStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly BookStoreDbContext db;
        private readonly IUserService userService;
        private readonly IShoppingCartService shoppingCartService;

        public OrderService(BookStoreDbContext db,IUserService userService, IShoppingCartService shoppingCartService)
        {
            this.db = db;
            this.userService = userService;
            this.shoppingCartService = shoppingCartService;
        }

       

        public async Task<bool> CreateAsync(string username)
        {
            var user = await userService.GetByUsernameAsync(username);

            if (user == null)
            {
                return false;
            }

            var shoppingCartBooks = db.ShoppingCartBooks
                .Include(shb => shb.Book)
                .Where(shb => shb.ShoppingCart.User.UserName == username && shb.Book.IsDeleted == false).ToList();

            if (shoppingCartBooks.Count() == 0)
            {
                return false;
            }

            var order = new Order
            {
                Status = OrderStatus.Completed,
                User = user,
                CompletionDate = DateTime.UtcNow
            };         

            List<OrderBook> orderBooks = new List<OrderBook>();

            foreach (var shoppingCartBook in shoppingCartBooks)
            {
                var orderBook = new OrderBook
                {
                    Order = order,
                    Book = shoppingCartBook.Book,
                    Quantity = shoppingCartBook.Quantity,
                    Price = shoppingCartBook.Book.Price
                };

                orderBooks.Add(orderBook);
            }
   
            order.OrderBooks = orderBooks;
            order.TotalPrice = orderBooks.Sum(o => o.Price * o.Quantity);
            
            await this.shoppingCartService.RemoveBooksFromShoppingCartAsync(username);

            this.db.Orders.Add(order);
            int result = db.SaveChanges();
            return result > 0;
            
        }

        public IQueryable<OrderListingServiceModel> GetUserOrders(string username)
        {
            return db.Orders
                .Where(o => o.User.UserName == username)
                .To<OrderListingServiceModel>();
        }

        public async Task<IEnumerable<OrderBookListingServiceModel>> GetOrderBooksAsync(int id)
        {
            return await db.OrderBooks
               .Where(ob => ob.OrderId == id)
               .Select(ob => new OrderBookListingServiceModel
               {
                   Id = ob.BookId,
                   ImageUrl = ob.Book.Image,
                   Title = ob.Book.Title,
                   Author = ob.Book.Author.FullName,
                   Price = ob.Book.Price,
                   Quantity = ob.Quantity,
                   TotalPrice = ob.Quantity * ob.Book.Price
               }).ToListAsync();
        }
    }
}
