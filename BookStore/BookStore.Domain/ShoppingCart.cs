using System.Collections.Generic;

namespace BookStore.Domain
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public BookStoreUser User { get; set; }

        public ICollection<ShoppingCartBook> ShoppingCartBooks { get; set; } = new List<ShoppingCartBook>();
    }
}
