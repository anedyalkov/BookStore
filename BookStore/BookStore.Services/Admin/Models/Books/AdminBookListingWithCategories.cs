using BookStore.Domain;
using BookStore.Services.Admin.Models.Categories;
using BookStore.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Admin.Models.Books
{
    public class AdminBookListingWithCategories : IMapFrom<Book>
    {
        public int Id { get; set; }

        public IEnumerable<AdminCategoryBasicServiceModel> CategoryBooks { get; set; } = new List<AdminCategoryBasicServiceModel>();
          
    }
}
