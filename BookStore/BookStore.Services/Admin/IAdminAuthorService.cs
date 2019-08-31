using BookStore.Services.Admin.Models.Authors;
using BookStore.Services.Admin.Models.Books;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services.Admin
{
    public interface IAdminAuthorService
    {
        IQueryable<AdminAuthorListingServiceModel> GetAllAuthors();

        IQueryable<AdminAuthorListingServiceModel> GetAllActiveAuthors();

        Task<bool> CreateAsync(string firstName, string lastName);

        Task<AdminAuthorListingServiceModel> GetByIdAsync(int id);

        Task<bool> EditAsync(int id, string firstName, string lastName);

        Task<bool> HideAsync(int id);

        Task<bool> ShowAsync(int id);
    }
}
