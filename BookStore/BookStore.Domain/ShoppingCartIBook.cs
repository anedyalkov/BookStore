using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Domain
{
    public class ShoppingCartBook
    {
        public int ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }

        public int Quantity { get; set; }

    }
}
