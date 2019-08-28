using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Admin;
using BookStore.Services.Mapping;
using BookStore.Services.Models.Books;
using BookStore.Services.Models.Categories;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services
{
    public class BookService : IBookService
    {
        private readonly BookStoreDbContext db;

        public BookService(BookStoreDbContext db)
        {
            this.db = db;
        }
        public IQueryable<BookListingServiceModel> GetAllActiveBooks()
        {
            return db.Books
                .Where(b => b.IsDeleted == false)
                .To<BookListingServiceModel>();
        }

        public async Task<BookDetailsServiceModel> GetByIdAsync(int id)
        {
            return await db.Books
              .Where(b => b.Id == id)
              .To<BookDetailsServiceModel>()
              .FirstOrDefaultAsync();
        }

        public IQueryable<BookListingServiceModel> GetBooksFilter(int? categoryId)
        {
            if (categoryId != null)
            {
                return this.GetBooksByCategory(categoryId.Value);
            }

            return this.GetAllActiveBooks();
        }

        public IQueryable<BookListingServiceModel> GetBooksByCategory(int categoryId)
        {
            var books = db.Books.Where(b => b.CategoryBooks.Any(cb => cb.CategoryId == categoryId)).To<BookListingServiceModel>();

            return books.Where(b => b.IsDeleted == false);
        }

        public IQueryable<BookListingServiceModel> FindBooks(string searchText)
        {
            searchText = searchText ?? string.Empty;

            return this.db
            .Books
            .Where(b => b.IsDeleted == false)
            .Where(b => b.Title.ToLower().Contains((searchText).ToLower()) 
            || b.Publisher.Name.ToLower().Contains((searchText).ToLower()) 
            || b.Author.FullName.ToLower().Contains((searchText).ToLower()))
            .To<BookListingServiceModel>();
        }

        public IQueryable<BookListingServiceModel> FindBooksByAuthor(string author)
        {
           return this.db
           .Books
           .Where(b => b.Author.FullName == author)
           .To<BookListingServiceModel>();
        }

        public IQueryable<BookListingServiceModel> FindBooksByPublisher(string publisher)
        {
          return this.db
          .Books
          .Where(b => b.Publisher.Name == publisher)
          .To<BookListingServiceModel>();
        }
    }
}
