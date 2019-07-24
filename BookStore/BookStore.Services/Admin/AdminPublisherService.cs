using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Admin.Models.Publishers;
using BookStore.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Admin
{
    public class AdminPublisherService : IAdminPublisherService
    {
        private readonly BookStoreDbContext db;

        public AdminPublisherService(BookStoreDbContext db)
        {
            this.db = db;
        }

        public IQueryable<AdminPublisherListingServiceModel> GetAllPublishers()
        {
            return db.Publishers.To<AdminPublisherListingServiceModel>();
        }

        public IQueryable<AdminPublisherBasicServiceModel> GetAllAvailablePublishers()
        {
            return db.Publishers
                .Where(p => p.IsDeleted == false)
                .OrderBy(p => p.Name)
                .To<AdminPublisherBasicServiceModel>();
        }

        public async Task<bool> CreateAsync(string name)
        {
            var publisher = new Publisher()
            {
                Name = name
            };

            db.Publishers.Add(publisher);
            int result = await db.SaveChangesAsync();
            return result > 0;

        }

        public async Task<AdminPublisherDetailsServiceModel> GetByIdAsync<AdminPublisherDetailsServiceModel>(int id)
        {
            return await this.db.Publishers
                  .Where(c => c.Id == id)
                  .To<AdminPublisherDetailsServiceModel>()
                  .FirstOrDefaultAsync();
        }


        public async Task<bool> EditAsync(int id, string name)
        {
            int result = 0;
            var publisher = db.Publishers.Find(id);

            if (publisher == null)
            {
                return result > 0;
            }

            publisher.Name = name;
            db.Publishers.Update(publisher);
            result = await db.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> HideAsync(int id)
        {
            int result = 0;
            var publisher = this.db.Publishers.FirstOrDefault(x => x.Id == id);

            if (publisher == null)
            {
                return result > 0;
            }

            publisher.IsDeleted = true;
            publisher.DeletedOn = DateTime.UtcNow;
            db.Publishers.Update(publisher);
            result = await db.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> ShowAsync(int id)
        {
            int result = 0;
            var publisher = this.db.Publishers.FirstOrDefault(x => x.Id == id);

            if (publisher == null)
            {
                return result > 0;
            }

            publisher.IsDeleted = false;
            publisher.DeletedOn = null;
            db.Publishers.Update(publisher);
            result = await db.SaveChangesAsync();

            return result > 0;
        }

      
    }
}
