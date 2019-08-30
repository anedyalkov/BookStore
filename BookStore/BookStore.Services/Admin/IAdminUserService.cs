using BookStore.Services.Admin.Models.Users;
using System.Linq;

namespace BookStore.Services.Admin
{
    public interface IAdminUserService
    {
        IQueryable<AdminUserListingServiceModel> GetAllUsers();
    }
}
