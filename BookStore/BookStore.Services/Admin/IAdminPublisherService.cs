using BookStore.Domain;
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
       
        IQueryable<AdminPublisherListingServiceModel> GetAllAvailablePublishers();

        Task<bool> CreateAsync(string name);

        Task<AdminPublisherListingServiceModel> GetByIdAsync(int id);

        Task<bool> EditAsync(int id, string name);

        Task<bool> HideAsync(int id);

        Task<bool> ShowAsync(int id);
    }
}
