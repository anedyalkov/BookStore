﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Domain;
using BookStore.Services.Admin.Models.Books;
using BookStore.Services.Admin.Models.Categories;
using BookStore.Services.Mapping;
using Microsoft.EntityFrameworkCore;

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
                .Include(b => b.Author)
                .OrderBy(b => b.Author.FullName)
                .To<AdminBookListingServiceModel>();
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
            var categoryBooks = this.db.CategoryBooks.Find(categoryId,bookId);

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
            await this.db.SaveChangesAsync();
            return true;
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
