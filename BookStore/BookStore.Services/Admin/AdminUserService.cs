using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Services.Mapping;
using BookStore.Services.Admin.Models.Users;

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
