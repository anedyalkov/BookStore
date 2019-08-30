using BookStore.Services.Admin.Models.Reviews;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services.Admin
{
    public interface IAdminReviewService
    {
        Task<AdminReviewListingServiceModel> GetByIdAsync(int id);
        IQueryable<AdminReviewListingServiceModel> GetAllReviews();
        Task<bool> RemoveAsync(int id);
    }
}
