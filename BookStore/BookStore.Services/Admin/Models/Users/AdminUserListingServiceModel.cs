using BookStore.Domain;
using BookStore.Services.Mapping;
using System.Collections.Generic;

namespace BookStore.Services.Admin.Models.Users
{
    public class AdminUserListingServiceModel : IMapFrom<BookStoreUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
