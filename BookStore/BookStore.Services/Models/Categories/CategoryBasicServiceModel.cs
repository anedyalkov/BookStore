using BookStore.Domain;
using BookStore.Services.Mapping;

namespace BookStore.Services.Models.Categories
{
    public class CategoryBasicServiceModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
