using BookStore.Domain;
using BookStore.Services.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public interface ICategoryService
    {
         IQueryable<CategoryListingServiceModel> GetAllActiveCategories();
         Task<TModel> GetByIdAsync<TModel>(int id);
    }
}
