using BookStore.Services.Admin.Models.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Admin
{
    public interface IAdminAuthorService
    {
        IQueryable<AdminAuthorListingServiceModel> GetAllAuthors();

        Task<bool> CreateAsync(string firstName, string lastName);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<bool> EditAsync(int id, string firstName, string lastName);

        Task<bool> HideAsync(int id);

        Task<bool> ShowAsync(int id);
    }
}
