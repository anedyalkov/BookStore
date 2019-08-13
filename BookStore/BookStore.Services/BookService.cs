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
using BookStore.Services.Models.Publishers;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services
{
    public class BookService : IBookService
    {
        private readonly BookStoreDbContext db;
        private readonly ICategoryService categoryService;
        private readonly IPublisherService publisherService;

        public BookService(BookStoreDbContext db,
            ICategoryService categoryService,
            IPublisherService publisherService)
        {
            this.db = db;
            this.categoryService = categoryService;
            this.publisherService = publisherService;
        }
        public IQueryable<BookListingServiceModel> GetAllActiveBooks()
        {
            return db.Books
                .Where(b => b.IsDeleted == false)
                .To<BookListingServiceModel>();
        }

        public async Task<TModel> GetById<TModel>(int id) where TModel : class
        {
            return await db.Books
              .Where(b => b.Id == id)
              .To<TModel>()
              .FirstOrDefaultAsync();
        }
        public async Task<IQueryable<BookListingServiceModel>> GetBooksFilter(int? categoryId)
        {

            if (categoryId != null)
            {
                return await this.GetBooksByCategory(categoryId.Value);
            }

            return this.GetAllActiveBooks();
        }

        public async Task<IQueryable<BookListingServiceModel>> GetBooksByCategory(int categoryId)
        {
            var category = await categoryService.GetByIdAsync<CategoryListingServiceModel>(categoryId);

            var books = (IQueryable<BookListingServiceModel>)category.Books.AsQueryable();

            return books.Where(b => b.IsDeleted == false);
        }

        public async Task<TModel> Details<TModel>(int id) where TModel : class
        {
            return await db.Books
               .Where(b => b.Id == id)
               .To<TModel>()
               .FirstOrDefaultAsync();
        }

        public IQueryable<BookListingServiceModel> FindBooks(string searchText)
        {
            searchText = searchText ?? string.Empty;

            return this.db
            .Books
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
