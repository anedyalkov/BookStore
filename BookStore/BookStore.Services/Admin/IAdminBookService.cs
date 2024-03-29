﻿using BookStore.Services.Admin.Models.Books;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services.Admin
{
    public interface IAdminBookService
    {
        IQueryable<AdminBookListingServiceModel> GetAllBooks();

        IQueryable<AdminBookListingServiceModel> GetBooksByPublisherId(int publisherId);

        IQueryable<AdminBookListingServiceModel> GetBooksByAuthorId(int authorId);

        IQueryable<AdminBookListingServiceModel> GetBooksByCategoryId(int categoryId);

        Task<bool> CreateAsync(
            string title,
            int authorId,
            int publisherId,
            string language,
            string description,
            string image,
            DateTime createdOn,
            decimal price
            );

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<bool> AddCategoryAsync(int id, int categoryId);

        Task<bool> RemoveCategoryAsync(int bookId, int categoryId);

        Task<bool> EditAsync(
           int id,
           string title,
           int authorId,
           int publisherId,
           string language,
           string description,
           string image,
           DateTime createdOn,
           decimal price
           );

        Task<bool> HideAsync(int id);

        Task<bool> ShowAsync(int id);
    }
}
