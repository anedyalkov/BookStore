﻿using BookStore.Domain;
using BookStore.Services.Mapping;
using BookStore.Services.Models.Reviews;
using System.Collections.Generic;

namespace BookStore.Web.Models.Books
{
    public class BookDetailsViewModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AuthorFullName { get; set; }

        public string PublisherName { get; set; }

        public string Language { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public IEnumerable<ReviewListingServiceModel> Reviews { get; set; }
    }
}
