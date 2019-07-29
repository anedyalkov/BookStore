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

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService bookService;
        private readonly ICategoryService categoryService;

        public HomeController(IBookService bookService,ICategoryService categoryService)
        {
            this.bookService = bookService;
            this.categoryService = categoryService;
        }
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            //if (this.User.Identity.IsAuthenticated)
            //{
            //    var books = await this.bookService.GetAllActiveBooks().ToListAsync();

            //    return this.View(books);
            //}

            if (this.User.Identity.IsAuthenticated)
            {
                var books = await this.bookService.GetBooksFilter(model.CategoryId).ToListAsync();
                var categories = await this.categoryService.GetAllActiveCategories().ToListAsync();

                model.Books = books;
                model.Categories = categories;

                return this.View(model);
            }

            return this.View();
        }
    }
}
