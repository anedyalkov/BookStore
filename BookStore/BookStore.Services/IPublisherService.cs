using BookStore.Services.Models.Publishers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public interface IPublisherService
    {
        Task<PublisherBasicServiceModel> GetPublisherBySearchTextAsync(string searchtext);
    }
}
