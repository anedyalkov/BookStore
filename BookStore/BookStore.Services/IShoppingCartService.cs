using BookStore.Services.Models.ShoppingCart;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public interface IShoppingCartService
    {
        Task<bool> AddBookToShoppingCartAsync(int id, string username);

        Task<bool> RemoveBookFromShoppingCartAsync(int id, string username);

        Task<IEnumerable<ShoppingCartServiceModel>> GetUserCartBooksAsync(string username);

        Task<bool> IncreaseQuantityAsync(int id, string username);

        Task<bool> DecreaseQuantityAsync(int id, string username);

        Task<bool> RemoveBooksFromShoppingCartAsync(string username);
    }
}
