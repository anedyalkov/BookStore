using BookStore.Domain;
using BookStore.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Admin.Models.Users
{
    public class AdminUserListingServiceModel : IMapFrom<BookStoreUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

    }
}
