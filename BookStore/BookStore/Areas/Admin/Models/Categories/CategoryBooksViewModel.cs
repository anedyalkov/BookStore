using BookStore.Services.Admin.Models.Books;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Models.Categories
{
    public class CategoryBooksViewModel
    {
        public AdminBookListingServiceModel Book { get; set; }
        public IEnumerable<SelectListItem> BookCategories { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> AllCategories { get; set; } = new List<SelectListItem>();
    }
}
