using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Mapping;
using BookStore.Services.Models.Books;
using BookStore.Services.Models.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class ReviewService : IReviewService
    {
        private readonly BookStoreDbContext db;
        private readonly IBookService bookService;

        public ReviewService(BookStoreDbContext db, IBookService bookService)
        {
            this.db = db;
            this.bookService = bookService;
        }
        public async Task<bool> CreateAsync(int bookId, string text, string creatorId)
        {
            var review = new Review
            {
                BookId = bookId,
                Text = text,
                CreatorId = creatorId,
                CreatedOn = DateTime.UtcNow
            };

            db.Reviews.Add(review);
            int result = await db.SaveChangesAsync();

            return result > 0;

        }

        public async Task<IQueryable<ReviewListingServiceModel>> GetReviewsByBook(int bookId)
        {
            var book = await bookService.GetById<BookListingServiceModel>(bookId);

            var reviews = book.Reviews.AsQueryable();

            return reviews
                .Where(r => r.IsDeleted == false)
                .OrderByDescending(r => r.CreatedOn);
        }
    }
}
