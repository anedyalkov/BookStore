using BookStore.Services;
using BookStore.Web.Models.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService bookService;
        private readonly ICategoryService categoryService;

        public HomeController(IBookService bookService, ICategoryService categoryService)
        {
            this.bookService = bookService;
            this.categoryService = categoryService;
        }
        public async Task<IActionResult> Index(IndexViewModel model, string author, string publisher)
        {

            if (this.User.Identity.IsAuthenticated)
            {
                if (author != null)
                {
                    var authorBooks = await this.bookService.FindBooksByAuthor(author).ToListAsync();
                    var categories = await this.categoryService.GetAllActiveCategories().ToListAsync();
                    model.Books = authorBooks;
                    model.Categories = categories;
                }
                else if (publisher != null)
                {
                    var publisherBooks = await this.bookService.FindBooksByPublisher(publisher).ToListAsync();
                    var categories = await this.categoryService.GetAllActiveCategories().ToListAsync();
                    model.Books = publisherBooks;
                    model.Categories = categories;
                }
                else
                {
                    var books = await this.bookService.GetBooksFilter(model.CategoryId).ToListAsync();
                    var categories = await this.categoryService.GetAllActiveCategories().ToListAsync();

                    model.Books = books;
                    model.Categories = categories;
                }
               
                return this.View(model);
            }

            return this.View();
        }

        public async Task<IActionResult> Search(string searchText)
        {
            var categories = await this.categoryService.GetAllActiveCategories().ToListAsync();
            var viewModel = new SearchViewModel
            {
                SearchText = searchText
            };

            var books = await this.bookService.FindBooks(searchText).ToListAsync();
            viewModel.Books = books;
            viewModel.Categories = categories;

            return View(viewModel);
        }
    }
}
