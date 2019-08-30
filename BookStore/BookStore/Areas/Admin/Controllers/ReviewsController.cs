using BookStore.Services.Admin;
using BookStore.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Controllers
{
    public class ReviewsController : AdminController
    {
        private readonly IAdminReviewService reviewService;

        public ReviewsController(IAdminReviewService reviewService)
        {
            this.reviewService = reviewService;
        }
        public async Task<IActionResult> Index()
        {
            var reviews = await this.reviewService.GetAllReviews().ToListAsync();

            return this.View(reviews);
        }

        public async Task<IActionResult> Delete(int id, int bookId)
        {
            var review = await this.reviewService
              .GetByIdAsync(id);

            var result = await this.reviewService
                .RemoveAsync(id);

            if (result == false)
            {
                return RedirectToAction(nameof(Index));
            }

            this.TempData.AddSuccessMessage(string.Format(
               WebAdminConstants.ReviewDeleted));

            return this.RedirectToAction(nameof(Web.Controllers.BooksController.Details), "Books", new { id = review.BookId });
        }
    }
}