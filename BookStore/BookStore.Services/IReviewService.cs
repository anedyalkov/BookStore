using BookStore.Services.Models.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public interface IReviewService
    {
        Task<bool> CreateAsync(int bookId,
            string text,
            string creatorId
            );

        IQueryable<ReviewListingServiceModel> GetReviewsByBook(int bookId);
    }
}
