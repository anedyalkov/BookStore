using BookStore.Services.Models.Books;
using BookStore.Services.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Models.Home
{
    public class IndexViewModel
    {
        public int? CategoryId { get; set; }
        public IList<BookListingServiceModel> Books { get; set; }
        public IList<CategoryListingServiceModel> Categories { get; set; }
    }
}
