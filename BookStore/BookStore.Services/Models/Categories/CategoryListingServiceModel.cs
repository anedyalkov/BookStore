using AutoMapper;
using BookStore.Domain;
using BookStore.Services.Mapping;
using BookStore.Services.Models.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Services.Models.Categories
{
    public class CategoryListingServiceModel : IMapFrom<Category>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<BookListingServiceModel> Books { get; set; } = new List<BookListingServiceModel>();

        public void CreateMappings(IProfileExpression configuration)
        {
            //var id = default(int);

            configuration
                .CreateMap<Category, CategoryListingServiceModel>()
                .ForMember(dest => dest.Books, opts => opts.MapFrom(src => src.CategoryBooks
                //.Where(c => c.BookId == id)
                .Select(cb => cb.Book)));
        }
    }
}
