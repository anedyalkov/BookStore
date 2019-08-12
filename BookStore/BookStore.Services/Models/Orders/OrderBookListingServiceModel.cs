using AutoMapper;
using BookStore.Domain;
using BookStore.Services.Infrastructure.Extensions;
using BookStore.Services.Mapping;
using System;


namespace BookStore.Services.Models.Orders
{
    public class OrderBookListingServiceModel 
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
