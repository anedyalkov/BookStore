using BookStore.Domain;
using BookStore.Services.Models.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public interface IShoppingCartService
    {
        Task<bool> AddBookToShoppingCart(int id, string username);
        Task<bool> RemoveBookFromShoppingCart(int id, string username);
        Task<IEnumerable<ShoppingCartServiceModel>> GetUserCartBooks(string username);
        //IEnumerable<ShoppingCartBook> GetUserCartBooks(string username);

        Task<bool> IncreaseQuantity(int id, string username);
        Task<bool> DecreaseQuantity(int id, string username);
        Task<bool> RemoveBooksFromShoppingCart(string username);
    }
}
