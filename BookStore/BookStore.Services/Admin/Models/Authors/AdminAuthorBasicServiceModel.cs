using BookStore.Domain;
using BookStore.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Admin.Models.Authors
{
    public class AdminAuthorBasicServiceModel : IMapFrom<Author>
    {
        public int Id { get; set; }

        public string FullName { get; set; }
    }
}
