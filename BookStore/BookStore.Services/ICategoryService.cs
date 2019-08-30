using BookStore.Services.Models.Categories;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public interface ICategoryService
    {
         IQueryable<CategoryListingServiceModel> GetAllActiveCategories();

         Task<CategoryListingServiceModel> GetByIdAsync(int id);
    }
}
