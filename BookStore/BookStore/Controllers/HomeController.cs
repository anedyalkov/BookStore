using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.Services;
using Microsoft.EntityFrameworkCore;
using BookStore.Web.Models.Home;
using BookStore.Services.Models.Publishers;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService bookService;
        private readonly ICategoryService categoryService;
        private readonly IPublisherService publisherService;

        public HomeController(IBookService bookService, ICategoryService categoryService, IPublisherService publisherService)
        {
            this.bookService = bookService;
            this.categoryService = categoryService;
            this.publisherService = publisherService;
        }
        public async Task<IActionResult> Index(IndexViewModel model, string author, string publisher)
        {
            //if (this.User.Identity.IsAuthenticated)
            //{
            //    var books = await this.bookService.GetAllActiveBooks().ToListAsync();

            //    return this.View(books);
            //}

            if (this.User.Identity.IsAuthenticated)
            {
                if (author != null)
                {
                    var authorBooks = await this.bookService.FindBooksByAuthor(author).ToListAsync();
                    var categories = await this.categoryService.GetAllActiveCategories().ToListAsync();
                    model.Books = authorBooks.ToList();
                    model.Categories = categories;
                }
                else if (publisher != null)
                {
                    var publisherBooks = await this.bookService.FindBooksByPublisher(publisher).ToListAsync();
                    var categories = await this.categoryService.GetAllActiveCategories().ToListAsync();
                    model.Books = publisherBooks.ToList();
                    model.Categories = categories;
                }
                else
                {
                    var books = await this.bookService.GetBooksFilter(model.CategoryId);
                    var categories = await this.categoryService.GetAllActiveCategories().ToListAsync();

                    model.Books = books.ToList();
                    model.Categories = categories;
                }
               

                return this.View(model);
            }

            return this.View();
        }

        public async Task<IActionResult> Search(string searchText = null)
        {
            var categories = await this.categoryService.GetAllActiveCategories().ToListAsync();
            var viewModel = new SearchViewModel
            {
                SearchText = /*searchModel.SearchText*/searchText
            };

            var books = await this.bookService.FindBooks(/*searchModel.SearchText*/searchText).ToListAsync();
            viewModel.Books = books;
            viewModel.Categories = categories;


            return View(viewModel);
        }
    }
}
