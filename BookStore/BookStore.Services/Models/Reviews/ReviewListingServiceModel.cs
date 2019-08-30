using BookStore.Domain;
using BookStore.Services.Mapping;
using System;

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
