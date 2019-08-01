﻿using BookStore.Services.Models.Books;
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

        Task<TModel> GetByIdAsync<TModel>(int id) where TModel : class;

        Task<TModel> Details<TModel>(int id) where TModel : class;

        Task<IQueryable<BookListingServiceModel>> GetBooksFilter(int? categoryId);

        Task<IQueryable<BookListingServiceModel>> GetBooksByCategory(int categoryId);
    }
}
