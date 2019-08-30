using BookStore.Domain;
using BookStore.Services.Mapping;

namespace BookStore.Services.Admin.Models.Authors
{
    public class AdminAuthorListingServiceModel : IMapFrom<Author>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsDeleted { get; set; }
    }
}
