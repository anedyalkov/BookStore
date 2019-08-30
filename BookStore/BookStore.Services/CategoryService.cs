using BookStore.Data;
using BookStore.Services.Mapping;
using BookStore.Services.Models.Categories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly BookStoreDbContext db;

        public CategoryService(BookStoreDbContext db)
        {
            this.db = db;
        }
        public IQueryable<CategoryListingServiceModel> GetAllActiveCategories()
        {
            return db.Categories
               .Where(c => c.IsDeleted == false)
               .To<CategoryListingServiceModel>();
        }

        public async Task<CategoryListingServiceModel> GetByIdAsync(int id)
        {
            return await this.db.Categories
                  .Where(c => c.Id == id)
                  .To<CategoryListingServiceModel>(new { id })
                  .FirstOrDefaultAsync();
        }
    }
}
