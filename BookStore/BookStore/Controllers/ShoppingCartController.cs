using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {

        private readonly IShoppingCartService shoppingCartservice;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartservice = shoppingCartService;
        }
        public async Task<IActionResult> Index()
        {
            var userCartBooks = await this.shoppingCartservice.GetUserCartBooksAsync(this.User.Identity.Name);
            return View(userCartBooks);
        }

        public async Task<IActionResult> AddToShoppingCart(int id)
        {
            await this.shoppingCartservice.AddBookToShoppingCartAsync(id, User.Identity.Name);
            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> IncreaseQuantity(int id)
        {
            await this.shoppingCartservice.IncreaseQuantityAsync(id, User.Identity.Name);

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DecreaseQuantity(int id)
        {
            await this.shoppingCartservice.DecreaseQuantityAsync(id, User.Identity.Name);

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveFromShoppingCart(int id)
        {
            await this.shoppingCartservice.RemoveBookFromShoppingCartAsync(id, User.Identity.Name);

            return this.RedirectToAction(nameof(Index));
        }
    }
}