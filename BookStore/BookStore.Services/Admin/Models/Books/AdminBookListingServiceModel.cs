using BookStore.Domain;
using BookStore.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Admin.Models.Books
{
    public class AdminBookListingServiceModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AuthorFullName { get; set; }

        public bool IsDeleted { get; set; }
    }
}
