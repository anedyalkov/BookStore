using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Models.Categories
{
    public class CategoryToBookInputModel
    {
        public int CategoryId { get; set; }
        public int BookId { get; set; }
    }
}
