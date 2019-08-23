using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Admin.Models.Authors;
using BookStore.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services.Admin
{
    public class AdminAuthorService : IAdminAuthorService
    {
        private readonly BookStoreDbContext db;

        public AdminAuthorService(BookStoreDbContext db)
        {
            this.db = db;
        }

        public IQueryable<AdminAuthorListingServiceModel> GetAllAuthors()
        {
            return db.Authors.To<AdminAuthorListingServiceModel>();
        }

        public IQueryable<AdminAuthorListingServiceModel> GetAllAvailableAuthors()
        {
            return db.Authors
                .Where(a => a.IsDeleted == false)
                .OrderBy(a => a.FullName)
                .To<AdminAuthorListingServiceModel>();
        }

        public async Task<bool> CreateAsync(string firstName, string lastName)
        {
            var author = new Author()
            {
                FirstName = firstName,
                LastName = lastName
            };

            db.Authors.Add(author);
            int result = await db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<AdminAuthorListingServiceModel> GetByIdAsync(int id)
        {
            return await this.db.Authors
                 .Where(c => c.Id == id)
                 .To<AdminAuthorListingServiceModel>()
                 .FirstOrDefaultAsync();
        }

        public async Task<bool> EditAsync(int id, string firstName, string lastName)
        {
            var author = db.Authors.Find(id);

            if (author == null)
            {
                return false;
            }

            author.FirstName = firstName;
            author.LastName = lastName;
            db.Authors.Update(author);
            int result = await db.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> HideAsync(int id)
        {
            var author = this.db.Authors
                .Include(a => a.Books)
                .FirstOrDefault(x => x.Id == id);

            if (author == null || author.Books.Any(b => b.IsDeleted == false))
            {
                return false;
            }

            author.IsDeleted = true;
            author.DeletedOn = DateTime.UtcNow;
            db.Authors.Update(author);
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ShowAsync(int id)
        {
            var author = this.db.Authors.FirstOrDefault(x => x.Id == id);

            if (author == null)
            {
                return false;
            }

            author.IsDeleted = false;
            author.DeletedOn = null;
            db.Authors.Update(author);
            int result = await db.SaveChangesAsync();

            return result > 0;
        }
    }
}
