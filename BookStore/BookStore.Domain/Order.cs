using BookStore.Domain.Enums;
using System;
using System.Collections.Generic;

namespace BookStore.Domain
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public BookStoreUser User { get; set; }

        public OrderStatus Status { get; set; }

        public decimal DeliveryPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime CompletionDate { get; set; }

        public ICollection<OrderBook> OrderBooks { get; set; } = new List<OrderBook>();
    }
}
