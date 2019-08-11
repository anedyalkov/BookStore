using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services.Models.ShoppingCart
{
    public class ShoppingCartServiceModel
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
