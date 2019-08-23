using BookStore.Domain;
using BookStore.Services.Mapping;
using BookStore.Services.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Models.Publishers
{
    public class PublisherBasicServiceModel : IMapFrom<Publisher>
    {
        public IEnumerable<BookListingServiceModel> Books { get; set; } /*= new List<BookListingServiceModel>();*/
    }
}
