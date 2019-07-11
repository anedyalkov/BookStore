using BookStore.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.CategoryNameMaxLength)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<CategoryBook> CategoryBooks { get; set; } = new List<CategoryBook>();
    }
}
