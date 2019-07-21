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

        public async Task<bool> Hide(int id)
        {
            int result = 0;
            var category = this.db.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                 return result > 0;
            }

            category.IsDeleted = true;
            category.DeletedOn = DateTime.UtcNow;
            db.Categories.Update(category);
            result = await db.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> Show(int id)
        {
            int result = 0;
            var category = this.db.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return result > 0;
            }

            category.IsDeleted = false;
            category.DeletedOn = null;
            db.Categories.Update(category);
            result = await db.SaveChangesAsync();

            return result > 0;
        }

        public async Task EditAsync(int id, string name)
        {
            var category = db.Categories.Find(id);

            if (category == null)
            {
                return;
            }

            category.Name = name;
            db.Categories.Update(category);
            await db.SaveChangesAsync();
        }

        public IQueryable<AdminCategoryListingServiceModel> GetAllCategories()
        {
            return db.Categories.To<AdminCategoryListingServiceModel>();
        }

        public async Task<AdminCategoryDetailsServiceModel> GetByIdAsync<AdminCategoryDetailsServiceModel>(int id)
        {
           return await this.db.Categories
                 .Where(c => c.Id == id)
                 .To<AdminCategoryDetailsServiceModel>()
                 .FirstOrDefaultAsync();
        }
    }
}
