using BookStore.Services.Models.Reviews;
using System.Linq;
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
