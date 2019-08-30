using BookStore.Domain;
using BookStore.Services.Mapping;

namespace BookStore.Services.Admin.Models.Categories
{
    public class AdminCategoryListingServiceModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }        
    }
}
