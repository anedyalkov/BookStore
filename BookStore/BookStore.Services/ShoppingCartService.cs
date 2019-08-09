using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Models.ShoppingCart;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly BookStoreDbContext db;
        private readonly IBookService bookService;
        private readonly IUserService userService;

        public ShoppingCartService(BookStoreDbContext db ,IBookService bookService,IUserService userService)
        {
            this.db = db;
            this.bookService = bookService;
            this.userService = userService;
        }
        public async Task<bool> AddBookToShoppingCart(int id, string username)
        {
            var book = this.db.Books.Find(id);
            var user = await this.userService.GetByUsername(username);
            var shoppingCart = this.db.ShoppingCarts.Find(user.ShoppingCartId);

            if (book == null || user == null)
            {
                return false;
            }

            var shoppingCartBook = GetShoppingCartBook(id, user.ShoppingCartId);

            if (shoppingCartBook != null)
            {
                return false;
            }

            shoppingCart.ShoppingCartBooks.Add(new ShoppingCartBook
            {
                BookId = id,
                Quantity = 1
            });

            int result = await this.db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<ShoppingCartServiceModel>> GetUserCartBooks(string username)
        {
            return await db.ShoppingCartBooks
                .Include(shb => shb.Book)
                .ThenInclude(b => b.Image)
                .Include(shb => shb.ShoppingCart)
                .Where(shb => shb.ShoppingCart.User.UserName == username)
                .Select(shb => new ShoppingCartServiceModel
                {
                    Id = shb.BookId,
                    ImageUrl = shb.Book.Image,
                    Title = shb.Book.Title,
                    Price = shb.Book.Price,
                    Quantity = shb.Quantity,
                    TotalPrice = shb.Book.Price
                }).ToListAsync();
        }

        private ShoppingCartBook GetShoppingCartBook(int bookId, int shoppingCartId)
        {
            return this.db.ShoppingCartBooks.FirstOrDefault(shb => shb.ShoppingCartId == shoppingCartId && shb.BookId == bookId);
        }
    }
}
