using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Admin.Models.Books;
using BookStore.Services.Admin.Models.Categories;
using BookStore.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services.Admin
{
    public class AdminBookService : IAdminBookService
    {
        private readonly BookStoreDbContext db;

        public AdminBookService(BookStoreDbContext db)
        {
            this.db = db;
        }
        public async Task<bool> CreateAsync(
            string title,
            int authorId,
            //int categoryId,
            int publisherId,
            string language,
            string description,
            DateTime createdOn,
            decimal price)
        {
            var book = new Book()
            {
                Title = title,
                AuthorId = authorId,
                PublisherId = publisherId,
                Language = language,
                Description = description,
                CreatedOn = createdOn,
                Price = price
            };

            db.Books.Add(book);
            int result = await db.SaveChangesAsync();
            return result > 0;
        }

        public IQueryable<AdminBookListingServiceModel> GetAllBooks()
        {
            return db.Books.To<AdminBookListingServiceModel>();
        }

        public async Task<TModel> GetByIdAsync<TModel>(int id) where TModel : class
        {
            return await db.Books
                 .Where(b => b.Id == id)
                 .To<TModel>()
                 .FirstOrDefaultAsync();
        }

        public async Task<bool> AddCategoryAsync(int bookId, int categoryId)
        {
          
            var category = this.db.Categories.Find(categoryId);
            var book = this.db.Books.Find(bookId);
            var categoryBooks = this.db.CategoryBooks.Find(categoryId,bookId);

            if (book == null
                || category == null
                || categoryBooks != null)
            {
                return false;
            }

            book.CategoryBooks.Add(new CategoryBook
            {
                CategoryId = categoryId
            });

            await this.db.SaveChangesAsync();
            return true;
        }

        //public Task <AdminBookListingServiceModel> GetBookCategoriesById(int id) 
        //{
        //    var result = db.Books
        //        .Where(b => b.Id == id)
        //        .To<AdminBookListingServiceModel>(new { id })
        //        .FirstOrDefaultAsync();


        //    return result;
        //}

        public async Task<bool> RemoveCategoryAsync(int bookId, int categoryId)
        {
            var booCategory = this.db.CategoryBooks.Find(categoryId, bookId);
            if (booCategory == null)
            {
                return false;
            }

            this.db.Remove(booCategory);
            await this.db.SaveChangesAsync();
            return true;
        }
    }
}
