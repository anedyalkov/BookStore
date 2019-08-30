using BookStore.Services.Models.Books;
using BookStore.Services.Models.Categories;
using System.Collections.Generic;

namespace BookStore.Web.Models.Home
{
    public class IndexViewModel : SearchInputModel
    {
        public int? CategoryId { get; set; }

        public IList<BookListingServiceModel> Books { get; set; }

        public IList<CategoryListingServiceModel> Categories { get; set; }
    }
}
