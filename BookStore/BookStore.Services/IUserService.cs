using BookStore.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public interface IUserService
    {
        Task<BookStoreUser> GetByUsernameAsync(string username);
    }
}
