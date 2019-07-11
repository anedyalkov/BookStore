using BookStore.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.AuthorNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(DataConstants.AuthorNameMaxLength)]
        public string LastName { get; set; }

        public string FullName
        {
            get { return $"{this.FirstName} {this.LastName}"; }
        }
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
