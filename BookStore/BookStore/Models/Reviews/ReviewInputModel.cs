using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Models.Reviews
{
    public class ReviewInputModel
    {
        public int BookId { get; set; }

        public string Text { get; set; }
    }
}
