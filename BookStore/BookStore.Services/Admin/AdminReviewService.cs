﻿using BookStore.Data;
using BookStore.Services.Admin.Models.Reviews;
using BookStore.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services.Admin
{
    public class AdminReviewService : IAdminReviewService
    {
        private readonly BookStoreDbContext db;

        public AdminReviewService(BookStoreDbContext db)
        {
            this.db = db;
        }

        public async Task<AdminReviewListingServiceModel> GetByIdAsync(int id)
        {
            return await db.Reviews
                 .Where(b => b.Id == id)
                 .To<AdminReviewListingServiceModel>()
                 .FirstOrDefaultAsync();
        }

        public IQueryable<AdminReviewListingServiceModel> GetAllReviews()
        {
            return db.Reviews
                .To<AdminReviewListingServiceModel>();
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var review = this.db.Reviews.Find(id);

            if (review == null)
            {
                return false;
            }

            this.db.Reviews.Remove(review);
            int result = await this.db.SaveChangesAsync();
            return result > 0;
        }
    }
}
