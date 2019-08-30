using BookStore.Services.Models.Orders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public interface IOrderService
    {
        Task<bool> CreateAsync(string username);

        IQueryable<OrderListingServiceModel> GetUserOrders(string username);

        Task<IEnumerable<OrderBookListingServiceModel>> GetOrderBooksAsync(int id);
    }
}
