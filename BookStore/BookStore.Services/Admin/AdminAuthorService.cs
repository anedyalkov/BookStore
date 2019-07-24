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

        public IQueryable<TModel> GetAllAuthors<TModel>() where TModel : class
        {
            return db.Authors.To<TModel>();
        }

        public IQueryable<TModel> GetAllAvailableAuthors<TModel>() where TModel : class
        {
            return db.Authors
                .Where(a => a.IsDeleted == false)
                .OrderBy(a => a.FullName)
                .To<TModel>();
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

        public async Task<TModel> GetByIdAsync<TModel>(int id) where TModel : class
        {
            return await this.db.Authors
                 .Where(c => c.Id == id)
                 .To<TModel>()
                 .FirstOrDefaultAsync();
        }

        public async Task<bool> EditAsync(int id, string firstName, string lastName)
        {
            int result = 0;
            var author = db.Authors.Find(id);

            if (author == null)
            {
                return result > 0;
            }

            author.FirstName = firstName;
            author.LastName = lastName;
            db.Authors.Update(author);
            result = await db.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> HideAsync(int id)
        {
            int result = 0;
            var author = this.db.Authors.FirstOrDefault(x => x.Id == id);

            if (author == null)
            {
                return result > 0;
            }

            author.IsDeleted = true;
            author.DeletedOn = DateTime.UtcNow;
            db.Authors.Update(author);
            result = await db.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> ShowAsync(int id)
        {
            int result = 0;
            var author = this.db.Authors.FirstOrDefault(x => x.Id == id);

            if (author == null)
            {
                return result > 0;
            }

            author.IsDeleted = false;
            author.DeletedOn = null;
            db.Authors.Update(author);
            result = await db.SaveChangesAsync();

            return result > 0;
        }
    }
}
