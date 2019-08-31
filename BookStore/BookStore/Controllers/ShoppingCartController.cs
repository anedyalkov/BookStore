using BookStore.Services;
using BookStore.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            var result = await this.shoppingCartservice.AddBookToShoppingCartAsync(id, User.Identity.Name);
       
            if (result == false)
            {
                this.TempData.AddErrorMessage(WebConstants.BookExistInCart);
                return RedirectToAction(nameof(Index));
            }

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
            var result  = await this.shoppingCartservice.RemoveBookFromShoppingCartAsync(id, User.Identity.Name);

            if (result == false)
            {
                this.TempData.AddErrorMessage(WebConstants.BookNotRemovedFromCart);
                return RedirectToAction(nameof(Index));
            }

            return this.RedirectToAction(nameof(Index));
        }
    }
}