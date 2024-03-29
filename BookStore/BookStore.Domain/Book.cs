﻿using BookStore.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.BookTitleMaxLength)]
        public string Title { get; set; }

        public int AuthorId { get; set; }

        public Author Author { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Image { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedOn { get; set; }

        public int PublisherId { get; set; }

        public Publisher Publisher { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<CategoryBook> CategoryBooks { get; set; } = new List<CategoryBook>();

        public ICollection<ShoppingCartBook> ShoppingCartBooks { get; set; } = new List<ShoppingCartBook>();

        public ICollection<OrderBook> OrderBooks { get; set; } = new List<OrderBook>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
