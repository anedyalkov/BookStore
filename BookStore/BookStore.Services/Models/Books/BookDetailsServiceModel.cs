﻿using AutoMapper;
using BookStore.Domain;
using BookStore.Services.Mapping;
using BookStore.Services.Models.Categories;
using BookStore.Services.Models.Reviews;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Services.Models.Books
{
    public class BookDetailsServiceModel : IMapFrom<Book>,  IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AuthorFullName { get; set; }

        public string PublisherName { get; set; }

        public string Language { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int ReviewsCount { get; set; }

        public IEnumerable<ReviewListingServiceModel> Reviews { get; set; }

        public IEnumerable<CategoryBasicServiceModel> Categories { get; set; }
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
               .CreateMap<Book, BookDetailsServiceModel>()
               .ForMember(dest => dest.Categories, opts => opts.MapFrom(src => src.CategoryBooks
               .Select(c => c.Category)));
        }
    }
}
