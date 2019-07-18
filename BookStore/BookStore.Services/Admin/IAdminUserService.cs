using BookStore.Services.Admin.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Admin
{
    public interface IAdminUserService
    {
        IQueryable<AdminUserListingServiceModel> GetAllUsers();
    }
}
