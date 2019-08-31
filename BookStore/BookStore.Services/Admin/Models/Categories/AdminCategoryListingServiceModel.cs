using AutoMapper;
using BookStore.Domain;
using BookStore.Services.Admin.Models.Books;
using BookStore.Services.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Services.Admin.Models.Categories
{
    public class AdminCategoryListingServiceModel : IMapFrom<Category>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public IEnumerable<AdminBookListingServiceModel> Books { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Category, AdminCategoryListingServiceModel>()
                .ForMember(dest => dest.Books, opts => opts.MapFrom(src => src.CategoryBooks
                .Select(c => c.Book)));
        }
    }
}
