using BookStore.Services.Admin.Models.Publishers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Admin
{
    public interface IAdminPublisherService
    {
        IQueryable<AdminPublisherListingServiceModel> GetAllPublishers();

        Task<bool> CreateAsync(string name);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<bool> EditAsync(int id, string name);

        Task<bool> HideAsync(int id);

        Task<bool> ShowAsync(int id);
    }
}
