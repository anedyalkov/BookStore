using BookStore.Controllers;
using BookStore.Services;
using BookStore.Web.Infrastructure.Extensions;
using BookStore.Web.Models.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    [Authorize]
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
                this.TempData.AddErrorMessage(WebConstants.BookNotFound);
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

            var result  = await this.reviewService.CreateAsync(reviewModel.BookId, reviewModel.Text, creatorId);

            if (result == false)
            {
                this.TempData.AddErrorMessage(WebConstants.ReviewNotCreated);
                return RedirectToAction(nameof(Details));
            }

            return this.RedirectToAction(nameof(Details), new { id = reviewModel.BookId });
        }
    }
}