using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BookStore.Domain
{
    public class BookStoreUser: IdentityUser
    {
        public int ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
