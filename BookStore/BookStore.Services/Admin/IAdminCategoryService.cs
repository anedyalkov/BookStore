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

        Task<bool> CreateAsync(string name);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task EditAsync(int id, string name);

        Task<bool> Hide(int id);

        Task<bool> Show(int id);
    }
}
