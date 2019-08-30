using BookStore.Domain;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public interface IUserService
    {
        Task<BookStoreUser> GetByUsernameAsync(string username);
    }
}
