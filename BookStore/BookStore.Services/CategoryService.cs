using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Services.Mapping;
using BookStore.Services.Models.Categories;
using Microsoft.EntityFrameworkCore;

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

        public  CategoryListingServiceModel GetById(int id)
        {
            return this.db.Categories
                  .Where(c => c.Id == id)
                  .To<CategoryListingServiceModel>(new { id })
                  .FirstOrDefault();
        }
    }
}
