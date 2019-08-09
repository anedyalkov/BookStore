using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService shoppingCartservice;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartservice = shoppingCartService;
        }
        public async Task<IActionResult> Index()
        {
            var userCartBooks = await this.shoppingCartservice.GetUserCartBooks(this.User.Identity.Name);
            return View(userCartBooks);
        }

        public async Task<IActionResult> AddToShoppingCart(int id)
        {
            await this.shoppingCartservice.AddBookToShoppingCart(id, User.Identity.Name);
            return this.RedirectToAction(nameof(Index));
        }
    }
}