using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.Services;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService bookService;

        public HomeController(IBookService bookService)
        {
            this.bookService = bookService;
        }
        public async Task<IActionResult> Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var books = await this.bookService.GetAllActiveBooks().ToListAsync();

                return this.View(books);
            }

            return this.View();
        }
    }
}
