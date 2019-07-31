using BookStore.Services;
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
        private readonly ICloudinaryService cloudinaryService;

        public BooksController(IAdminBookService bookService,
            IAdminAuthorService authorService,
            IAdminPublisherService publisherService,
            IAdminCategoryService categoryService,
            ICloudinaryService cloudinaryService)
        {
            this.bookService = bookService;
            this.authorService = authorService;
            this.publisherService = publisherService;
            this.categoryService = categoryService;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<IActionResult> Index()
        {
            var books = await this.bookService.GetAllBooks().ToListAsync();

            return this.View(books);
        }

        public async Task<IActionResult> Create()
        {
            return this.View(new BookInputModel()
            {
                Authors = await this.GetAuthorsAsync(),
                Publishers = await this.GetPublishersAsync()
            });
        }


        [HttpPost]
        public async Task<IActionResult> Create(BookInputModel bookModel)
        {
            if (!this.ModelState.IsValid)
            {
                bookModel.Authors = await this.GetAuthorsAsync();
                bookModel.Publishers = await this.GetPublishersAsync();
                return this.View(bookModel);
            }
            string imageUrl = await this.cloudinaryService.UploadPictureAsync(
               bookModel.Image,
               bookModel.Title);

            await this.bookService.CreateAsync(
                bookModel.Title,
                bookModel.AuthorId,
                bookModel.PublisherId,
                bookModel.Language,
                bookModel.Description,
                imageUrl,
                bookModel.CreatedOn,
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

            return this.View(new BookCategoriesViewModel
            {
                Book = book,
                BookCategories = book.Categories
                .Select(b => new SelectListItem
                {
                    Text = b.Name,
                    Value = b.Id.ToString()
                })
                .ToList(),
                AllCategories = await GetCategoriesAsync()
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

            var success =  await this.bookService.AddCategoryAsync(
                model.BookId,
                model.CategoryId);


            if (!success)
            {
                this.TempData.AddErrorMessage(string.Format(
                    WebAdminConstants.CategoryAlreadyAddedToBookMsg,
                     category.Name,
                     book.Title));

                return this.RedirectToAction(nameof(Categories), new { id = model.BookId });
            }

            this.TempData.AddSuccessMessage(string.Format(
                WebAdminConstants.CategoryAddedToBookMsg,
                category.Name,
                book.Title));

            return this.RedirectToAction(nameof(Categories), new { id = model.BookId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCategoryFromBook(CategoryToBookInputModel model)
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


             await this.bookService.RemoveCategoryAsync(
                model.BookId,
                model.CategoryId);


            this.TempData.AddSuccessMessage(string.Format(
                WebAdminConstants.CategoryRemovedFromBookMsg,
                category.Name,
                book.Title));

            return this.RedirectToAction(nameof(Categories), new { id = model.BookId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var book = await this.bookService
                .GetByIdAsync<AdminBookDetailsServiceModel>(id);

            if (book == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.BookNotFoundMsg);
                return RedirectToAction(nameof(Index));
            }

            return this.View(new BookInputModel
            {
               Title = book.Title,
               AuthorId = book.AuthorId,
               Authors = await GetAuthorsAsync(),
               PublisherId = book.PublisherId,
               Publishers = await GetPublishersAsync(),
               Language = book.Language,
               Description = book.Description,
               Image = book.Image,
               CreatedOn = book.CreatedOn,
               Price = book.Price
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BookInputModel bookModel)
        {
            // TODO : check if book exists
            if (!this.ModelState.IsValid)
            {
                return this.View(bookModel);
            }
            string imageUrl = await this.cloudinaryService.UploadPictureAsync(
             bookModel.Image,
             bookModel.Title);

            await this.bookService.EditAsync(
                id,
                  bookModel.Title,
                bookModel.AuthorId,
                bookModel.PublisherId,
                bookModel.Language,
                bookModel.Description,
                imageUrl,
                bookModel.CreatedOn,
                bookModel.Price
                );

            this.TempData.AddSuccessMessage(string.Format(
                WebAdminConstants.BookUpdatedMsg,
                bookModel.Title));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Hide(int id)
        {
            var book = await this.bookService
                .GetByIdAsync<AdminBookDetailsServiceModel>(id);

            if (book == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.BookNotFoundMsg);
                return RedirectToAction(nameof(Index));
            }

            await this.bookService.HideAsync(id);


            this.TempData.AddSuccessMessage(string.Format(
              WebAdminConstants.BookHiddenMsg,
              book.Title));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Show(int id)
        {
            var book = await this.bookService
             .GetByIdAsync<AdminBookDetailsServiceModel>(id);

            if (book == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.BookNotFoundMsg);
                return RedirectToAction(nameof(Index));
            }

            await this.bookService.ShowAsync(id);

            this.TempData.AddSuccessMessage(string.Format(
              WebAdminConstants.BookShowedMsg,
              book.Title));

            return this.RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetAuthorsAsync()
        {
            var result = await this.authorService.GetAllAvailableAuthors<AdminAuthorBasicServiceModel>()
            .Select(a => new SelectListItem
            {
                Text = a.FullName,
                Value = a.Id.ToString()
            })
            .OrderBy(a => a.Text)
          .ToListAsync();

            return result;
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
