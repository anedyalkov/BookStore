using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public interface IPublisherService
    {
        Task<TModel> GetPublisherBySerchText<TModel>(string searchtext);
    }
}
