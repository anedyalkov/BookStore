using BookStore.Services.Models.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public interface IBookService
    {
        IQueryable<BookListingServiceModel> GetAllActiveBooks();

        Task<TModel> GetById<TModel>(int id) where TModel : class;

        Task<TModel> Details<TModel>(int id) where TModel : class;

        Task<IQueryable<BookListingServiceModel>> GetBooksFilter(int? categoryId);

        Task<IQueryable<BookListingServiceModel>> GetBooksByCategory(int categoryId);

        IQueryable<BookListingServiceModel> FindBooks(string searchText);
        IQueryable<BookListingServiceModel> FindBooksByAuthor(string author);
        IQueryable<BookListingServiceModel> FindBooksByPublisher(string publisher);
    }
}
