using AutoMapper;
using BookStore.Domain;
using BookStore.Services.Mapping;
using System;

namespace BookStore.Services.Admin.Models.Reviews
{
    public class AdminReviewListingServiceModel : IMapFrom<Review> , IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Creator { get; set; }

        public int BookId { get; set; }

        public string Book { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Review, AdminReviewListingServiceModel>()
                .ForMember(dest => dest.Creator, opts => opts.MapFrom(src => src.Creator.UserName))
                .ForMember(dest => dest.Book, opts => opts.MapFrom(src => src.Book.Title));
        }
    }
}
