
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain
{
    public class BookStoreUser: IdentityUser
    {
        public int ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
