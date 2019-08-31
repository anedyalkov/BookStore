using BookStore.Domain;
using BookStore.Services.Admin.Models.Books;
using BookStore.Services.Mapping;
using System.Collections.Generic;

namespace BookStore.Services.Admin.Models.Publishers
{
    public class AdminPublisherListingServiceModel : IMapFrom<Publisher>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public IEnumerable<AdminBookListingServiceModel> Books { get; set; }
    }
}
