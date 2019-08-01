using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Domain
{
    public class Review
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string CreatorId { get; set; }

        public BookStoreUser Creator { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
