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
            return db.Publishers
                .OrderBy(c => c.Name)
                .To<AdminPublisherListingServiceModel>();
        }

        public IQueryable<AdminPublisherListingServiceModel> GetAllActivePublishers()
        {
            return db.Publishers
                .Where(p => p.IsDeleted == false)
                .OrderBy(p => p.Name)
                .To<AdminPublisherListingServiceModel>();
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

        public async Task<AdminPublisherListingServiceModel> GetByIdAsync(int id)
        {
            return await this.db.Publishers
                  .Where(c => c.Id == id)
                  .To<AdminPublisherListingServiceModel>()
                  .FirstOrDefaultAsync();
        }


        public async Task<bool> EditAsync(int id, string name)
        {
            var publisher = db.Publishers.Find(id);

            if (publisher == null)
            {
                return false;
            }

            publisher.Name = name;
            db.Publishers.Update(publisher);
            int result = await db.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> HideAsync(int id)
        {
            var publisher = this.db.Publishers
                .Include(p => p.Books)
                .FirstOrDefault(x => x.Id == id);

            if (publisher == null || publisher.Books.Any(b => b.IsDeleted == false))
            {
                return false;
            }

            publisher.IsDeleted = true;
            publisher.DeletedOn = DateTime.UtcNow;
            db.Publishers.Update(publisher);
            int result = await db.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> ShowAsync(int id)
        {
            var publisher = this.db.Publishers.FirstOrDefault(x => x.Id == id);

            if (publisher == null)
            {
                return false;
            }

            publisher.IsDeleted = false;
            publisher.DeletedOn = null;
            db.Publishers.Update(publisher);
            int result = await db.SaveChangesAsync();

            return result > 0;
        }
    }
}
