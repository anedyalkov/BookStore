using BookStore.Domain;
using BookStore.Services.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public interface IOrderService
    {
        Task<Order> CreateAsync(string username);

        Task<bool> AddBooksToOrder(Order order, string username);

        IQueryable<OrderListingServiceModel> GetUserOrders(string username);

        Task<IEnumerable<OrderBookListingServiceModel>> GetOrderBooks(int id);
    }
}
