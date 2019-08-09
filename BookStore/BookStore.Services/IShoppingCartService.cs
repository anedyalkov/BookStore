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
        Task<IEnumerable<ShoppingCartServiceModel>> GetUserCartBooks(string username);
    }
}
