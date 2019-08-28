using BookStore.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookStore.Tests.Common
{
    public static class BookStoreDbContextInMemoryFactory
    {
        public static BookStoreDbContext InitializeContext()
        {
            var options = new DbContextOptionsBuilder<BookStoreDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            return new BookStoreDbContext(options);
        }
    }
}
