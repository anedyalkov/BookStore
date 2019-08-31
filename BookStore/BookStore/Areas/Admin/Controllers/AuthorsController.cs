using BookStore.Services.Admin;
using BookStore.Web.Areas.Admin.Models.Authors;
using BookStore.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Controllers
{
    public class AuthorsController : AdminController
    {
        private readonly IAdminAuthorService authorService;
        private readonly IAdminBookService bookService;

        public AuthorsController(IAdminAuthorService authorService, IAdminBookService bookService)
        {
            this.authorService = authorService;
            this.bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await this.authorService.GetAllAuthors().ToListAsync();

            return this.View(authors);
        }

        public IActionResult Create()
        {
            return this.View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(AuthorInputModel authorModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(authorModel);
            }

            var result = await this.authorService.CreateAsync(authorModel.FirstName, authorModel.LastName);

            if (result == false)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.AuthorNotCreated);
                return RedirectToAction(nameof(Index));
            }

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Books(int id)
        {
            var author = await this.authorService.GetByIdAsync(id);

            if (author == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.AuthorNotFound);
                return this.RedirectToAction(nameof(Index));
            }
            var books = bookService.GetBooksByAuthorId(id);
            return this.View(books);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var author = await this.authorService
                .GetByIdAsync(id);

            if (author == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.AuthorNotFound);
                return RedirectToAction(nameof(Index));
            }

            return this.View(new AuthorInputModel
            {
                FirstName = author.FirstName,
                LastName = author.LastName
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AuthorInputModel authorModel)
        {

            if (!this.ModelState.IsValid)
            {
                return this.View(authorModel);
            }

            var result = await this.authorService.EditAsync(
                id,
                authorModel.FirstName,
                authorModel.LastName);

            if (result == false)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.AuthorNotEdited);
                return RedirectToAction(nameof(Index));
            }

            this.TempData.AddSuccessMessage(string.Format(
                WebAdminConstants.AuthorUpdated,
                authorModel.FirstName,
                authorModel.LastName));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Hide(int id)
        {
            var author = await this.authorService
                .GetByIdAsync(id);

            if (author == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.AuthorNotFound);
                return RedirectToAction(nameof(Index));
            }

            var result  = await this.authorService.HideAsync(id);

            if (result == false)
            {
                this.TempData.AddErrorMessage(string.Format(WebAdminConstants.AuthorIncludesBooks,
                   $"{author.FirstName} {author.LastName}"));
                return RedirectToAction(nameof(Index));
            }

            this.TempData.AddSuccessMessage(string.Format(
              WebAdminConstants.AuthorHidden,
              author.FirstName,
              author.LastName));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Show(int id)
        {
            var author = await this.authorService
             .GetByIdAsync(id);

            if (author == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.AuthorNotFound);
                return RedirectToAction(nameof(Index));
            }

            await this.authorService.ShowAsync(id);

            this.TempData.AddSuccessMessage(string.Format(
              WebAdminConstants.AuthorShowed,
              author.FirstName,
              author.LastName));

            return this.RedirectToAction(nameof(Index));
        }
    }
}
