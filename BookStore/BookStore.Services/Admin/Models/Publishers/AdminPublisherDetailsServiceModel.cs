using BookStore.Domain;
using BookStore.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Admin.Models.Publishers
{
    public class AdminPublisherDetailsServiceModel : IMapFrom<Publisher>
    {
        public string Name { get; set; }
    }
}
