using AutoMapper;
using BookStore.Domain;
using BookStore.Services.Infrastructure.Extensions;
using BookStore.Services.Mapping;

using System;


namespace BookStore.Services.Models.Orders
{
    public class OrderListingServiceModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime CompletionDate { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Order, OrderListingServiceModel>()
                .ForMember(dest => dest.Status, opts => opts.MapFrom(src => src.Status.GetDisplayName()));

        }
    }
}
