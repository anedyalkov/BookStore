using BookStore.Services.Admin.Models.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Admin
{
    public interface IAdminBookService
    {
        IQueryable<AdminBookListingServiceModel> GetAllBooks();

        Task<bool> CreateAsync(
            string title,
            int authorId,
            //int categotyId,
            int publisherId,
            string language,
            string description,
            DateTime createdOn,
            decimal price
            );
        Task<TModel> GetByIdAsync<TModel>(int id) where TModel : class;

        Task<bool> AddCategoryAsync(int id, int categoryId);
    }
}
