using BookStore.Domain;
using BookStore.Services.Mapping;

namespace BookStore.Services.Admin.Models.Publishers
{
    public class AdminPublisherListingServiceModel : IMapFrom<Publisher>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }      
    }
}
