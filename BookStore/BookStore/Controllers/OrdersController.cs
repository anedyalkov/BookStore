using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Services;
using BookStore.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Web.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var result =  await this.orderService.CreateAsync(this.User.Identity.Name);

            if (result == false)
            {
                this.TempData.AddErrorMessage(WebConstants.OrderErrorMsg);
            }

            return this.RedirectToAction(nameof(ShoppingCartController.Index),
                "ShoppingCart");
        }

        public async Task<IActionResult> MyOrders()
        {
            var orders = await this.orderService.GetUserOrders(this.User.Identity.Name).ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var orderBooks = await this.orderService.GetOrderBooksAsync(id);

            return View(orderBooks);
        }
    }
}