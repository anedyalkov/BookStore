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
        public async Task<bool> AddBookToShoppingCartAsync(int id, string username)
        {
            var book = this.db.Books.Find(id);
            var user = await this.userService.GetByUsernameAsync(username);
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

        public async Task<bool> RemoveBookFromShoppingCartAsync(int id, string username)
        {
            var user = await this.userService.GetByUsernameAsync(username);

            var shoppingCartBook = GetShoppingCartBook(id, user.ShoppingCartId);

            if (shoppingCartBook == null)
            {
                return false;
            }

            db.ShoppingCartBooks.Remove(shoppingCartBook);
            var result = await db.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<ShoppingCartServiceModel>> GetUserCartBooksAsync(string username)
        {
            return await db.ShoppingCartBooks
                .Where(shb => shb.ShoppingCart.User.UserName == username && shb.Book.IsDeleted == false)
                .Select(shb => new ShoppingCartServiceModel
                {
                    Id = shb.BookId,
                    ImageUrl = shb.Book.Image,
                    Title = shb.Book.Title,
                    Author = shb.Book.Author.FullName,
                    Price = shb.Book.Price,
                    Quantity = shb.Quantity,
                    TotalPrice = shb.Quantity * shb.Book.Price
                }).ToListAsync();
        }

        public async Task<bool> IncreaseQuantityAsync(int id, string username)
        {
            var book = db.Books.Find(id);
            var user = await this.userService.GetByUsernameAsync(username);

            if (book == null || user == null)
            {
                return false;
            }

            var shoppingCartBook = GetShoppingCartBook(id, user.ShoppingCartId);

            if (shoppingCartBook == null)
            {
                return false;
            }

            shoppingCartBook.Quantity += 1;

            int result = await this.db.SaveChangesAsync();
            return result > 0;

        }

        public async Task<bool> DecreaseQuantityAsync(int id, string username)
        {
            var book = db.Books.Find(id);
            var user = await this.userService.GetByUsernameAsync(username);

            if (book == null || user == null)
            {
                return false;
            }

            var shoppingCartBook = GetShoppingCartBook(id, user.ShoppingCartId);

            if (shoppingCartBook == null)
            {
                return false;
            }

            var quantity = shoppingCartBook.Quantity;

            if (quantity == 1)
            {
                return false;
            }

            shoppingCartBook.Quantity -= 1;

            int result = await this.db.SaveChangesAsync();
            return result > 0;
        }

        private ShoppingCartBook GetShoppingCartBook(int bookId, int shoppingCartId)
        {
            return this.db.ShoppingCartBooks.FirstOrDefault(shb => shb.ShoppingCartId == shoppingCartId && shb.BookId == bookId);
        }

        public async Task<bool> RemoveBooksFromShoppingCartAsync(string username)
        {
            var user = await this.userService.GetByUsernameAsync(username);

            //if (user == null)
            //{
            //    return;
            //}

            var shoppingCartBooks = this.db.ShoppingCartBooks.Where(shb => shb.ShoppingCartId == user.ShoppingCartId);

            this.db.ShoppingCartBooks.RemoveRange(shoppingCartBooks);
            int result = this.db.SaveChanges();
            return result > 0;
        }
    }
}
