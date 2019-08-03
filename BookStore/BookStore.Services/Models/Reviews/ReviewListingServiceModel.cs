using BookStore.Domain;
using BookStore.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Models.Reviews
{
    public class ReviewListingServiceModel : IMapFrom<Review>
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string CreatorUserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
