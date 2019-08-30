using BookStore.Services.Admin.Models.Books;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BookStore.Web.Areas.Admin.Models.Books
{
    public class BookCategoriesViewModel
    {
        public AdminBookListingServiceModel Book { get; set; }

        public IEnumerable<SelectListItem> BookCategories { get; set; } 

        public IEnumerable<SelectListItem> AllCategories { get; set; } 
    }
}
