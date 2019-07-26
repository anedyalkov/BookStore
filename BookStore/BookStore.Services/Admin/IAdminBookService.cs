using BookStore.Services.Admin.Models.Books;
using BookStore.Services.Admin.Models.Categories;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        Task<bool> RemoveCategoryAsync(int bookId, int categoryId);
        //Task<AdminBookListingServiceModel>GetBookCategoriesById(int id) ;

    }
}
