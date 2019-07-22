using BookStore.Domain;
using BookStore.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Admin.Models.Authors
{
    public class AdminAuthorDetailsServiceModel : IMapFrom<Author>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
