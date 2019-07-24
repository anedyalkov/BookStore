using BookStore.Services.Admin;
using BookStore.Services.Admin.Models.Authors;
using BookStore.Web.Areas.Admin.Models.Authors;
using BookStore.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Controllers
{
    public class AuthorsController : AdminController
    {
        private readonly IAdminAuthorService authorService;

        public AuthorsController(IAdminAuthorService authorService)
        {
            this.authorService = authorService;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await this.authorService.GetAllAuthors<AdminAuthorListingServiceModel>().ToListAsync();

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

            await this.authorService.CreateAsync(authorModel.FirstName, authorModel.LastName);

            this.TempData.AddSuccessMessage(string.Format(
               WebAdminConstants.AuthorCreatedMsg,
               authorModel.FirstName,
               authorModel.LastName));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var author = await this.authorService
                .GetByIdAsync<AdminAuthorDetailsServiceModel>(id);

            if (author == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.AuthorNotFoundMsg);
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
            // TODO : check if uthor exists
            if (!this.ModelState.IsValid)
            {
                return this.View(authorModel);
            }

            await this.authorService.EditAsync(
                id,
                authorModel.FirstName,
                authorModel.LastName);

            this.TempData.AddSuccessMessage(string.Format(
                WebAdminConstants.AuthorUpdatedMsg,
                authorModel.FirstName,
                authorModel.LastName));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Hide(int id)
        {
            var author = await this.authorService
                .GetByIdAsync<AdminAuthorDetailsServiceModel>(id);

            if (author == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.AuthorNotFoundMsg);
                return RedirectToAction(nameof(Index));
            }

            await this.authorService.HideAsync(id);

            this.TempData.AddSuccessMessage(string.Format(
              WebAdminConstants.AuthorHiddenMsg,
              author.FirstName,
              author.LastName));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Show(int id)
        {
            var author = await this.authorService
             .GetByIdAsync<AdminAuthorDetailsServiceModel>(id);

            if (author == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.AuthorNotFoundMsg);
                return RedirectToAction(nameof(Index));
            }

            await this.authorService.ShowAsync(id);

            this.TempData.AddSuccessMessage(string.Format(
              WebAdminConstants.AuthorShowedMsg,
              author.FirstName,
              author.LastName));

            return this.RedirectToAction(nameof(Index));
        }
    }
}
