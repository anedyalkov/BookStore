using BookStore.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain
{
    public class Publisher
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.PublisherNameMaxLength)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
