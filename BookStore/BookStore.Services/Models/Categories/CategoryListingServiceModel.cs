using AutoMapper;
using BookStore.Domain;
using BookStore.Services.Mapping;
using BookStore.Services.Models.Books;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Services.Models.Categories
{
    public class CategoryListingServiceModel : IMapFrom<Category>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public IEnumerable<BookListingServiceModel> Books { get; set; } = new List<BookListingServiceModel>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Category, CategoryListingServiceModel>()
                .ForMember(dest => dest.Books, opts => opts.MapFrom(src => src.CategoryBooks
                .Select(cb => cb.Book)));
        }
    }
}
