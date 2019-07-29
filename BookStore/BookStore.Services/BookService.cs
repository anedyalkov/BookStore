using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookStore.Data;
using BookStore.Services.Mapping;
using BookStore.Services.Models.Books;
using BookStore.Services.Models.Categories;

namespace BookStore.Services
{
    public class BookService : IBookService
    {
        private readonly BookStoreDbContext db;
        private readonly ICategoryService categoryService;

        public BookService(BookStoreDbContext db, ICategoryService categoryService)
        {
            this.db = db;
            this.categoryService = categoryService;
        }
        public IQueryable<BookListingServiceModel> GetAllActiveBooks()
        {
            return db.Books
                .Where(b => b.IsDeleted == false)
                .To<BookListingServiceModel>();
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
            var category = categoryService.GetById(categoryId);

            var books = (IQueryable<BookListingServiceModel>)category.Books;

            return books;
        }
    }
}
