using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Admin.Models.Books;
using BookStore.Services.Admin.Models.Categories;
using BookStore.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services.Admin
{
    public class AdminCategoryService : IAdminCategoryService
    {

        private readonly BookStoreDbContext db;

        public AdminCategoryService(BookStoreDbContext db)
        {
            this.db = db;
        }

        public IQueryable<AdminCategoryListingServiceModel> GetAllCategories()
        {
            return db.Categories
                .To<AdminCategoryListingServiceModel>();
        }

        public IQueryable<AdminCategoryListingServiceModel> GetAllActiveCategories()
        {
            return db.Categories
                 .Where(c => c.IsDeleted == false)
                 .To<AdminCategoryListingServiceModel>();
        }

        public async Task<bool> CreateAsync(string name)
        {
            var category = new Category()
            {
                Name = name
            };

           db.Categories.Add(category);
           int result = await db.SaveChangesAsync();
           return result > 0;

        }

        public async Task<AdminCategoryListingServiceModel> GetByIdAsync(int id)
        {
            return await this.db.Categories
                  .Where(c => c.Id == id)
                  .To<AdminCategoryListingServiceModel>()
                  .FirstOrDefaultAsync();
        }


        public async Task<bool> EditAsync(int id, string name)
        {
            var category = db.Categories.Find(id);

            if (category == null)
            {
                return false;
            }

            category.Name = name;
            db.Categories.Update(category);
            int result = await db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> HideAsync(int id)
        {
            var category = this.db.Categories
                .FirstOrDefault(c => c.Id == id);

            var success = this.db.Categories
                .Where(c => c.Id == id)
                .SelectMany(cb => cb.CategoryBooks)
                .Select(x => x.Book)
                .Any(b => b.IsDeleted == false);

            if (category == null || success == true)
            {
                 return false;
            }

            category.IsDeleted = true;
            category.DeletedOn = DateTime.UtcNow;
            db.Categories.Update(category);
            int result = await db.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> ShowAsync(int id)
        {
            var category = this.db.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return false;
            }

            category.IsDeleted = false;
            category.DeletedOn = null;
            db.Categories.Update(category);
            int result = await db.SaveChangesAsync();

            return result > 0;
        }
    }
}
