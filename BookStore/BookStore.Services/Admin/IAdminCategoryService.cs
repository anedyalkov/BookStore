using BookStore.Services.Admin.Models.Categories;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services.Admin
{
    public interface IAdminCategoryService
    {
        IQueryable<AdminCategoryListingServiceModel> GetAllCategories();

        IQueryable<AdminCategoryListingServiceModel> GetAllActiveCategories();

        Task<bool> CreateAsync(string name);

        Task<AdminCategoryListingServiceModel> GetByIdAsync(int id);

        Task <bool>EditAsync(int id, string name);

        Task<bool> HideAsync(int id);

        Task<bool> ShowAsync(int id);
    }
}
