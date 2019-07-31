using AutoMapper;
using BookStore.Domain;
using BookStore.Services.Mapping;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Admin.Models.Books
{
    public class AdminBookDetailsServiceModel :  IMapFrom<Book>, IHaveCustomMappings
    {  
        public string Title { get; set; }

        public int AuthorId { get; set; }

        public int PublisherId { get; set; }

        public string Language { get; set; }

        public string Description { get; set; }

        public IFormFile Image { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Book, AdminBookDetailsServiceModel>()
                .ForMember(dest => dest.Image, opts => opts.Ignore());
        }
    }
}
