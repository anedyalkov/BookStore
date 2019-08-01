using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Controllers;
using BookStore.Services;
using BookStore.Services.Models.Books;
using BookStore.Web.Infrastructure.Extensions;
using BookStore.Web.Models.Books;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService bookService;

        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }
        public async Task<IActionResult> Details(int id)
        {
            var book = await this.bookService.GetByIdAsync<BookDetailsViewModel>(id);

            if (book == null)
            {
                this.TempData.AddErrorMessage(WebConstants.BookNotFoundMsg);
                return this.RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var bookDetails = await this.bookService.Details<BookDetailsViewModel>(id);

            return this.View(bookDetails);
        }
    }
}