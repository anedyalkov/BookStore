using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Admin.Models.Categories;
using BookStore.Services.Mapping;
using Microsoft.EntityFrameworkCore;

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
                .OrderBy(c => c.Name)
                .To<AdminCategoryListingServiceModel>();
        }


        public IQueryable<AdminCategoryListingServiceModel> GetAllActiveCategories()
        {
            return db.Categories
                 .Where(c => c.IsDeleted == false)
                 .OrderBy(c => c.Name)
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
            int result = 0;
            var category = db.Categories.Find(id);

            if (category == null)
            {
                return false;
            }

            category.Name = name;
            db.Categories.Update(category);
            result = await db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> HideAsync(int id)
        {
            var category = this.db.Categories
                .Include(c => c.CategoryBooks)
                .FirstOrDefault(x => x.Id == id);

            if (category == null || category.CategoryBooks.Any())
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
