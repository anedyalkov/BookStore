using BookStore.Services.Admin.Models.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Admin
{
    public interface IAdminReviewService
    {
         IQueryable<AdminReviewListingServiceModel> GetReviews();
         Task<bool> RemoveAsync(int id);
         Task<AdminReviewListingServiceModel> GetByIdAsync(int id);

    }
}
