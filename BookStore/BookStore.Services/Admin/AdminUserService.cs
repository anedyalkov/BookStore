using BookStore.Data;
using BookStore.Services.Admin.Models.Users;
using BookStore.Services.Mapping;
using System.Linq;

namespace BookStore.Services.Admin
{
    public class AdminUserService : IAdminUserService
    {
        private readonly BookStoreDbContext db;

        public AdminUserService(BookStoreDbContext db)
        {
            this.db = db;
        }
        public IQueryable<AdminUserListingServiceModel> GetAllUsers()
        {
            return db.Users.To<AdminUserListingServiceModel>();
        }
    }
}
