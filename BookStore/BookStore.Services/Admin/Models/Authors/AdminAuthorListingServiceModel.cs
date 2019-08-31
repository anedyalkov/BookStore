using BookStore.Domain;
using BookStore.Services.Admin.Models.Books;
using BookStore.Services.Mapping;
using System.Collections.Generic;

namespace BookStore.Services.Admin.Models.Authors
{
    public class AdminAuthorListingServiceModel : IMapFrom<Author>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsDeleted { get; set; }

        public IEnumerable<AdminBookListingServiceModel> Books { get; set; }
    }
}
