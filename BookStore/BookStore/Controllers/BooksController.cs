using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookStore.Controllers;
using BookStore.Services;
using BookStore.Services.Models.Books;
using BookStore.Web.Infrastructure.Extensions;
using BookStore.Web.Models.Books;
using BookStore.Web.Models.Reviews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService bookService;
        private readonly IReviewService reviewService;
        private readonly ICategoryService categoryService;

        public BooksController(IBookService bookService,IReviewService reviewService,ICategoryService categoryService)
        {
            this.bookService = bookService;
            this.reviewService = reviewService;
            this.categoryService = categoryService;
        }
        public async Task<IActionResult> Details(int id)
        {
            var book = await this.bookService.GetByIdAsync(id);

            if (book == null)
            {
                this.TempData.AddErrorMessage(WebConstants.BookNotFoundMsg);
                return this.RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var reviews = await reviewService.GetReviewsByBook(id).ToListAsync();
            book.Reviews = reviews;

            return this.View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Review(ReviewInputModel reviewModel)
        {
            var creatorId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await this.reviewService.CreateAsync(reviewModel.BookId, reviewModel.Text, creatorId);

            return this.RedirectToAction(nameof(Details), new { id = reviewModel.BookId });
        }
    }
}