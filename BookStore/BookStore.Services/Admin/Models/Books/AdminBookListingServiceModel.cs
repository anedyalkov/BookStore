﻿using AutoMapper;
using BookStore.Domain;
using BookStore.Services.Admin.Models.Categories;
using BookStore.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Services.Admin.Models.Books
{
    public class AdminBookListingServiceModel : IMapFrom<Book>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AuthorFullName { get; set; }

        public string PublisherName { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }

        public IEnumerable<AdminCategoryListingServiceModel> Categories { get; set; } 

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Book, AdminBookListingServiceModel>()
                .ForMember(dest => dest.Categories, opts => opts.MapFrom(src => src.CategoryBooks
                .Select(c => c.Category)));
        }
    }
}
