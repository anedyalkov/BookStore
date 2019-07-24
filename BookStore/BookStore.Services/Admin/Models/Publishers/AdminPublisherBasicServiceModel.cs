using BookStore.Domain;
using BookStore.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Admin.Models.Publishers
{
    public class AdminPublisherBasicServiceModel : IMapFrom<Publisher>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
