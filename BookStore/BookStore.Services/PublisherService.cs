using BookStore.Data;
using BookStore.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly BookStoreDbContext db;

        public PublisherService(BookStoreDbContext db)
        {
            this.db = db;
        }
        public async Task<PublisherBasicServiceModel> GetPublisherBySerchText<PublisherBasicServiceModel>(string searchText)
        {
            return await db.Publishers
                .Where(p => p.IsDeleted == false)
                .Where(p => p.Name.ToLower().Contains((searchText).ToLower()))
                .Include(p => p.Books)
                .ThenInclude(b => b.Reviews)
                .To<PublisherBasicServiceModel>()
                .FirstOrDefaultAsync();
        }
    }
}
