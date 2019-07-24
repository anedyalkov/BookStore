using BookStore.Services.Admin;
using BookStore.Services.Admin.Models.Authors;
using BookStore.Services.Admin.Models.Books;
using BookStore.Services.Admin.Models.Categories;
using BookStore.Web.Areas.Admin.Models.Books;
using BookStore.Web.Areas.Admin.Models.Categories;
using BookStore.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Controllers
{
    public class BooksController : AdminController
    {
        private readonly IAdminBookService bookService;
        private readonly IAdminAuthorService authorService;
        private readonly IAdminPublisherService publisherService;
        private readonly IAdminCategoryService categoryService;

        public BooksController(IAdminBookService bookService,
            IAdminAuthorService authorService,
            IAdminPublisherService publisherService,
            IAdminCategoryService categoryService)
        {
            this.bookService = bookService;
            this.authorService = authorService;
            this.publisherService = publisherService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await this.bookService.GetAllBooks().ToListAsync();

            return this.View(authors);
        }

        public async Task<IActionResult> Create()
        {
            return this.View(new BookInputModel()
            {
                Authors = await this.GetAuthorsAsync(),
                //Categories = await this.GetCategoriesAsync(),
                Publishers = await this.GetPublishersAsync()
            });
        }


        [HttpPost]
        public async Task<IActionResult> Create(BookInputModel bookModel)
        {
            if (!this.ModelState.IsValid)
            {
                bookModel.Authors = await this.GetAuthorsAsync();
                //bookModel.Categories = await this.GetCategoriesAsync();
                bookModel.Publishers = await this.GetPublishersAsync();
                return this.View(bookModel);
            }

            await this.bookService.CreateAsync(
                bookModel.Title,
                bookModel.AuthorId,
                bookModel.PublisherId,
                bookModel.Language,
                bookModel.Description,
                (DateTime)bookModel.CreatedOn,
                bookModel.Price
                );

            this.TempData.AddSuccessMessage(string.Format(
               WebAdminConstants.BookCreatedMsg,
               bookModel.Title));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Categories(int id)
        {
            var book = await this.bookService
               .GetByIdAsync<AdminBookListingServiceModel>(id);
            return this.View(new CategoryBooksViewModel
            {
                Book = book,
                Categories = await GetCategoriesAsync()
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddCategoryToBook(CategoryToBookInputModel model)
        {
            var book = await this.bookService.GetByIdAsync<AdminBookListingServiceModel>(model.BookId);
            if (book == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.BookNotFoundMsg);
                return this.RedirectToAction(nameof(Index));
            }

            var category = await this.categoryService.GetByIdAsync<AdminCategoryListingServiceModel>(model.CategoryId);

            if (category == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.CategoryNotFoundMsg);
                return this.RedirectToAction(nameof(Index));
            }

            //if (!this.ModelState.IsValid)
            //{
            //    this.TempData.AddErrorMessage(WebAdminConstants.ArtistInvalidDataMsg);
            //    return this.RedirectToAction(nameof(Artists), new { id = model.RecordingId });
            //}

           var success =  await this.bookService.AddCategoryAsync(
                model.BookId,
                model.CategoryId);


            if (!success)
            {
                this.TempData.AddErrorMessage(string.Format(
                    WebAdminConstants.CategoryAllreadyAddedToBookMsg,
                     category.Name,
                     book.Title));

                return this.RedirectToAction(nameof(Categories), new { id = model.CategoryId });
            }

            this.TempData.AddSuccessMessage(string.Format(
                WebAdminConstants.CategoryAddedToBookMsg,
                category.Name,
                book.Title));

            return this.RedirectToAction(nameof(Categories), new { id = model.CategoryId });
        }

        private async Task<IEnumerable<SelectListItem>> GetAuthorsAsync()
        {
            return await this.authorService.GetAllAvailableAuthors<AdminAuthorBasicServiceModel>()
            .Select(a => new SelectListItem
            {
                Text = a.FullName,
                Value = a.Id.ToString()
            })
          .ToListAsync();
        }

        private async Task<IEnumerable<SelectListItem>> GetCategoriesAsync()
        {
            return await this.categoryService.GetAllAvailableCategories()
            .Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
          .ToListAsync();
        }

        private async Task<IEnumerable<SelectListItem>> GetPublishersAsync()
        {
            return await this.publisherService.GetAllAvailablePublishers()
            .Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            })
          .ToListAsync();
        }
    }
}
