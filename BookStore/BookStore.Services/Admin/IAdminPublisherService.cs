using BookStore.Services.Admin.Models.Publishers;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services.Admin
{
    public interface IAdminPublisherService
    {
        IQueryable<AdminPublisherListingServiceModel> GetAllPublishers();
       
        IQueryable<AdminPublisherListingServiceModel> GetAllActivePublishers();

        Task<bool> CreateAsync(string name);

        Task<AdminPublisherListingServiceModel> GetByIdAsync(int id);

        Task<bool> EditAsync(int id, string name);

        Task<bool> HideAsync(int id);

        Task<bool> ShowAsync(int id);
    }
}
