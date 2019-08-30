using BookStore.Services.Models.Books;
using BookStore.Services.Models.Categories;
using System.Collections.Generic;

namespace BookStore.Web.Models.Home
{
    public class SearchViewModel
    {
        public string SearchText { get; set; }

        public IList<BookListingServiceModel> Books { get; set; }
           = new List<BookListingServiceModel>();

        public IList<CategoryListingServiceModel> Categories { get; set; }
        = new List<CategoryListingServiceModel>();

    }
}
