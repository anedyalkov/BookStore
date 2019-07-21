using BookStore.Domain;
using BookStore.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Admin.Models.Categories
{
    public class AdminCategoryDetailsServiceModel : IMapFrom<Category>
    {
        public string Name { get; set; }
    }
}
