using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Mapping;
using BookStore.Services.Models.Reviews;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class ReviewService : IReviewService
    {
        private readonly BookStoreDbContext db;

        public ReviewService(BookStoreDbContext db)
        {
            this.db = db;
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

        public IQueryable<ReviewListingServiceModel> GetReviewsByBook(int bookId)
        {
            var reviews = db.Reviews.Where(r => r.BookId == bookId).To<ReviewListingServiceModel>();

            return reviews
                .Where(r => r.IsDeleted == false)
                .OrderByDescending(r => r.CreatedOn);
        }
    }
}
