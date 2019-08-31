using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Admin.Models.Books;
using BookStore.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            int publisherId,
            string language,
            string description,
            string image,
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
                Image = image,
                CreatedOn = createdOn,
                Price = price
            };

            db.Books.Add(book);
            int result = await db.SaveChangesAsync();
            return result > 0;
        }

        public IQueryable<AdminBookListingServiceModel> GetAllBooks()
        {
            return db.Books
                .To<AdminBookListingServiceModel>();
        }

        public IQueryable<AdminBookListingServiceModel> GetBooksByPublisherId(int publisherId)
        {
            var books = db.Books.Where(b => b.PublisherId == publisherId).To<AdminBookListingServiceModel>();

            return books;
        }

        public IQueryable<AdminBookListingServiceModel> GetBooksByAuthorId(int authorId)
        {
            var books = db.Books.Where(b => b.AuthorId == authorId).To<AdminBookListingServiceModel>();

            return books;
        }

        public IQueryable<AdminBookListingServiceModel> GetBooksByCategoryId(int categoryId)
        {
            var books = db.Books.Where(b => b.CategoryBooks.Any(cb => cb.CategoryId == categoryId)).To<AdminBookListingServiceModel>();

            return books;
        }

        public async Task<TModel> GetByIdAsync<TModel>(int id)
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
            var categoryBooks = this.db.CategoryBooks.Find(categoryId, bookId);

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

           int result =  await this.db.SaveChangesAsync();
           return result > 0;
        }

        public async Task<bool> RemoveCategoryAsync(int bookId, int categoryId)
        {
            var bookCategory = this.db.CategoryBooks.Find(categoryId, bookId);
            if (bookCategory == null)
            {
                return false;
            }

            this.db.Remove(bookCategory);
            int result = await this.db.SaveChangesAsync();
            return result > 0;         
        }


        public async Task<bool> EditAsync(int id,
            string title,
            int authorId,
            int publisherId,
            string language,
            string description,
            string image,
            DateTime createdOn,
            decimal price)
        {

            var book = db.Books.Find(id);

            if (book == null)
            {
                return false;
            }

            book.Title = title;
            book.AuthorId = authorId;
            book.PublisherId = publisherId;
            book.Language = language;
            book.Description = description;
            book.Image = image;
            book.CreatedOn = createdOn;
            book.Price = price;
            db.Books.Update(book);
            int result = await db.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> HideAsync(int id)
        {
            var book = this.db.Books
                .FirstOrDefault(x => x.Id == id);

            if (book == null)
            {
                return false;
            }

            book.IsDeleted = true;
            book.DeletedOn = DateTime.UtcNow;
            db.Books.Update(book);
            var result =  await db.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> ShowAsync(int id)
        {
            var book = this.db.Books.FirstOrDefault(x => x.Id == id);

            if (book == null)
            {
                return false;
            }

            book.IsDeleted = false;
            book.DeletedOn = null;
            db.Books.Update(book);
            var result = await db.SaveChangesAsync();

            return result > 0;
        }
    }
}
