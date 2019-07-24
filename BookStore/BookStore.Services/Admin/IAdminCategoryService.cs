using BookStore.Services.Admin.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Admin
{
    public interface IAdminCategoryService
    {
        IQueryable<AdminCategoryListingServiceModel> GetAllCategories();
        IQueryable<AdminCategoryBasicServiceModel> GetAllAvailableCategories();

        Task<bool> CreateAsync(string name);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task <bool>EditAsync(int id, string name);

        Task<bool> HideAsync(int id);

        Task<bool> ShowAsync(int id);
    }
}
