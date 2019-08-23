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

        Task<BookDetailsServiceModel> GetByIdAsync(int id);

        IQueryable<BookListingServiceModel> GetBooksFilter(int? categoryId);

        IQueryable<BookListingServiceModel> GetBooksByCategory(int categoryId);

        IQueryable<BookListingServiceModel> FindBooks(string searchText);
        IQueryable<BookListingServiceModel> FindBooksByAuthor(string author);
        IQueryable<BookListingServiceModel> FindBooksByPublisher(string publisher);
    }
}
